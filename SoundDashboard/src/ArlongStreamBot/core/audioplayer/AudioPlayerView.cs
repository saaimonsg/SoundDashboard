using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core;
using Dapper;
using log4net;
using NAudio.Wave;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace ArlongStreambot
{
    public class AudioPlayerView
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AudioPlayerView));
        public List<SoundClipData> _clipList { get; }
        private AudioPlayerViewController _viewController;

        public AudioPlayerView()
        {
            _clipList = new List<SoundClipData>();
            _viewController = new AudioPlayerViewController();
        }
        
        public FlowLayoutPanel GetOutputDevicesLayout()
        {
            FlowLayoutPanel outputPanel = new FlowLayoutPanel();
            // _outputPanel.Width = ScreenController.minimumSize.Width - 74;
            outputPanel.Height = 28;
            // _outputPanel.BackColor = Color.Brown;
            outputPanel.WrapContents = false;
            outputPanel.Name = "outputPanel";
            outputPanel.Location = new System.Drawing.Point(0, 20);
            outputPanel.FlowDirection = FlowDirection.LeftToRight;
            // _outputPanel.TabIndex = 1;
            // _outputPanel.Padding = new Padding(10);

            Label outputLabel = new Label();
            outputLabel.Name = "outputLabel";
            outputLabel.Text = @"Output Device";
            // _outputLabel.TabIndex = 2;
            outputLabel.TextAlign = ContentAlignment.MiddleCenter;
            outputPanel.Controls.Add(outputLabel);


            ComboBox outputComboBox = new ComboBox();

            // _outputComboBox.Location = new System.Drawing.Point(0, 20);
            outputComboBox.Name = "outputComboBox";
            outputComboBox.Size = new System.Drawing.Size(200, 20);
            outputComboBox.DropDownStyle = ComboBoxStyle.DropDownList; //makes the dropdown not editable
            outputComboBox.SelectedIndexChanged += ChangeOutputDevice;

            //Outputs devices
            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                WaveOutCapabilities caps = WaveOut.GetCapabilities(n);
                outputComboBox.Items.Add($"{n}: {caps.ProductName}");
            }

            outputComboBox.SelectedIndex = 0;
            outputPanel.Controls.Add(outputComboBox);
            outputPanel.Width = outputLabel.Width + outputComboBox.Width + 15;
            return outputPanel;
        }

        public FlowLayoutPanel GetInputDevicesLayout()
        {
            FlowLayoutPanel inputPanel = new FlowLayoutPanel();
            inputPanel.Height = 22;
            // _inputPanel.BackColor = Color.Brown;
            inputPanel.WrapContents = false;
            inputPanel.Name = "outputPanel";
            inputPanel.Location = new System.Drawing.Point(0, 0);
            inputPanel.FlowDirection = FlowDirection.LeftToRight;
            
            Label inputLabel = new Label();
            inputLabel.Name = "inputLabel";
            inputLabel.Text = @"Input Device";
            inputLabel.TextAlign = ContentAlignment.MiddleCenter;
            inputPanel.Controls.Add(inputLabel);
            
            //TODO call fetchDevices with recursion in event handler on value change in ComboBox
            //so devices can be reloaded.
            ComboBox inputComboBox = new ComboBox();
            inputComboBox.Location = new System.Drawing.Point(0, 0);
            inputComboBox.Size = new System.Drawing.Size(200, 20);
            inputComboBox.DropDownStyle = ComboBoxStyle.DropDownList; //makes the dropdown not editable
            inputComboBox.SelectedIndexChanged += ChangeInputDevice;
            for (int n = -1; n < WaveIn.DeviceCount; n++)
            {
                WaveInCapabilities caps = WaveIn.GetCapabilities(n);
                inputComboBox.Items.Add($"{n}: {caps.ProductName}");
            }
            
            inputComboBox.SelectedIndex = 0;

            inputPanel.Width = inputLabel.Width + inputComboBox.Width + 15;
            inputPanel.Controls.Add(inputComboBox);
            return inputPanel;
        }

        private void ChangeOutputDevice(object sender, EventArgs e)
        {
            _viewController.ChangeDeviceOutput(GetDeviceIndex((ComboBox)sender));
        }

        private void ChangeInputDevice(object sender, EventArgs e)
        {
            //TODO implement audio input device switch
            // _audioPlayerViewHandler.changeDeviceInput(GetDeviceIndex((ComboBox)sender));
        }

        private static int GetDeviceIndex(ComboBox cb)
        {
            String senderSelectedItem = (String)cb.SelectedItem;
            var substring = senderSelectedItem.Substring(0, senderSelectedItem.IndexOf(':'));
            return int.Parse(substring);
        }

        public FlowLayoutPanel GetAudioPlayerButtonsLayout()
        {
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Location = new Point(0, 50);
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.BackColor = Color.Aquamarine;
            flowPanel.Margin = new Padding(0);
            flowPanel.Size = new System.Drawing.Size(ScreenController.minimumSize.Width - 74, ScreenController.minimumSize.Height - 40);
            flowPanel.AllowDrop = true;
            flowPanel.DragLeave += AudioClipDragLeave;
            flowPanel.DragOver += AudioClipDragOver;
            flowPanel.DragEnter += AudioClipDragEnter;
            flowPanel.DragDrop += AudioClipDragDrop;

            FetchAudioLibrary(flowPanel);
            
            return flowPanel;
        }

        private void FetchAudioLibrary(FlowLayoutPanel flowPanel)
        {
            using (var clipRepo = new SoundClipRepository())
            {
                List<SoundClip> list = clipRepo.FetchAll();
                foreach (SoundClip soundClip in list)
                {
                    Button btn2 = new Button();
                    btn2.Text = soundClip.display_name;
                    btn2.Click += _viewController.OnButtonPlayClickEventHandler2(soundClip.source_path);
                    flowPanel.Controls.Add(btn2);
                }
            }
        }

        private void AudioClipDragDrop(object sender, DragEventArgs e)
        {
            FlowLayoutPanel panel = (FlowLayoutPanel)sender;
            String[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            SoundClipRepository clipRepository = new SoundClipRepository();
            foreach (String sourceFilePath in filenames)
            {
                String filename = sourceFilePath.Split('\\').Last();
                String destinationFilePath = $"{ResourceHandler.soundFolder}\\{filename}";
                SoundClipData soundClipData = new SoundClipData(filename, sourceFilePath, destinationFilePath);
                
                Button btn2 = new Button();
                btn2.Text = soundClipData.FileName;
                btn2.Click += _viewController.OnButtonPlayClickEventHandler2(destinationFilePath);
                panel.Controls.Add(btn2);

                clipRepository.save(soundClipData.SoundClip);
                _viewController.SaveAudioFileDragAndDrop(soundClipData); 
            }
        }

        private void AudioClipDragEnter(object sender, DragEventArgs e)
        {
            var dataPresent = e.Data.GetDataPresent(DataFormats.FileDrop);
            var boolCheck = true;
            if (dataPresent)
            {
                String[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (String sourceFilePath in filenames)
                {
                    //TODO REVISAR
                    if (_viewController.ValidateAudioFormat(sourceFilePath) && boolCheck)
                    {
                        // _log.Info(sourceFilePath);
                    }
                    else
                    {
                        boolCheck = false;
                    }
                }
            }

            if (boolCheck)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                _log.Warn("The file or some of the files aren't supported");
            }
        }

        private void AudioClipDragLeave(object sender, EventArgs e)
        {
            // _log.Info("DragLeave");
            // _log.Info(sender);
            // _log.Info(e);
        }

        private void AudioClipDragOver(object sender, DragEventArgs e)
        {
            // _log.Info("DragOver");
            // _log.Info(sender);
            // _log.Info(e);
        }
    }
}