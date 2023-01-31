﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DAOs
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Category> GetCategoryList()
        {
            List<Category> categories;
            try
            {
                var myStoreDB = new FStoreDBContext();
                categories = myStoreDB.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categories;
        }

        public Category GetCategoryByID(int categoryID)
        {
            Category category = null;
            try
            {
                var myStoreDB = new FStoreDBContext();
                category = myStoreDB.Categories.SingleOrDefault(category => category.CategoryId == categoryID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public void AddCategory(Category category)
        {
            try
            {
                Category c = GetCategoryByID(category.CategoryId);
                if (c == null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Categories.Add(c);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The category has already existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                Category c = GetCategoryByID(category.CategoryId);
                if (c != null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Entry<Category>(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The category has not existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveCategory(Category category)
        {
            try
            {
                Category c = GetCategoryByID(category.CategoryId);
                if (c != null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Remove(c);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The category has not existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
