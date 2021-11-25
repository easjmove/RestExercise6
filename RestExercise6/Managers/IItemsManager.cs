using RestExercise6.Models;
using System.Collections.Generic;

namespace RestExercise6.Managers
{
    //This interface was extracted from the ItemsManager class
    //Nothing has been added manually
    public interface IItemsManager
    {
        Item Add(Item newItem);
        Item Delete(int id);
        IEnumerable<Item> GetAll(string substring = null, int minimumQuality = 0, int minimumQuantity = 0);
        IEnumerable<Item> GetAllBetweenQuality(int minQuality, int maxQuality);
        Item GetById(int id);
        Item Update(int id, Item updates);
    }
}