using System.Collections.Generic;

namespace ArlongStreambot.core.resource.sqlite
{
    public interface IRepository<T,ID>
    {
        T save(T t);
        T update(T t);
        T remove(ID id);
        
        T FindByID(ID id);
        T FindFirst();
        
        
        List<T> FetchAll();
        
    }
}
