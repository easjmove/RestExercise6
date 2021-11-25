using RestExercise6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestExercise6.Managers
{
    public class ItemsManagerDB : IItemsManager
    {
        //Remeber the Context from the constructor as it is used in all/most methods
        private ItemContext _context;

        //The constructor takes a Context from whoever initialized it
        public ItemsManagerDB(ItemContext context)
        {
            _context = context;
        }

        public Item Add(Item newItem)
        {
            //Setting the Id to 0, so the database doesn't try to insert the ID from the caller
            //it should be the database that assigns the Id's
            newItem.Id = 0;
            _context.Items.Add(newItem);
            //Remember to call the savechanges everytime you make changes
            _context.SaveChanges();
            return newItem;
        }

        public Item Delete(int id)
        {
            //Finds the Item that should be deleted using the Id
            //Uses the GetByID method because if we optimize this method, or change how to find a specific Item, we only need to implement this once
            Item itemToBeDeleted = GetById(id);
            _context.Items.Remove(itemToBeDeleted);
            //Remember to call the savechanges everytime you make changes
            _context.SaveChanges();
            return itemToBeDeleted;
        }

        public IEnumerable<Item> GetAll(string substring = null, int minimumQuality = 0, int minimumQuantity = 0)
        {
            //Here we check if the different parameter values is set and if so filter the list
            //It is always recommended to make the database do as much of the work as possible
            IEnumerable<Item> items = from item in _context.Items
                                      where (substring == null || item.Name.Contains(substring))
                                      && (minimumQuality == 0 || item.ItemQuality >= minimumQuality)
                                      && (minimumQuantity == 0 || item.Quantity >= minimumQuantity)
                                      select item;

            //Simply converts the DbSet to a List
            return items;
        }

        public IEnumerable<Item> GetAllBetweenQuality(int minQuality, int maxQuality)
        {
            //Here it asks for all items that have a quality between and including the parameters
            //It is always recommended to make the database do as much of the work as possible
            IEnumerable<Item> items = from item in _context.Items
                                      where item.ItemQuality >= minQuality && item.ItemQuality <= maxQuality
                                      select item;

            //Simply converts the DbSet to a List
            return items;
        }

        public Item GetById(int id)
        {
            //The find method looks for the primary key (id)
            return _context.Items.Find(id);
        }

        public Item Update(int id, Item updates)
        {
            //Finds the Item that should be updated using the Id
            //Uses the GetByID method because if we optimize this method, or change how to find a specific Item, we only need to implement this once
            Item itemToBeUpdated = GetById(id);

            //update the values
            //Notice we don't update the Id, as it comes from the first parameter, and if the id is different in the updates, it gets ignored
            //We want the database to handle the Id's for us
            itemToBeUpdated.Name = updates.Name;
            itemToBeUpdated.ItemQuality = updates.ItemQuality;
            itemToBeUpdated.Quantity = updates.Quantity;

            //Remember to call the savechanges everytime you make changes
            //In this case it can see that the itemToBeUpdated has been updated
            _context.SaveChanges();

            return itemToBeUpdated;
        }
    }
}
