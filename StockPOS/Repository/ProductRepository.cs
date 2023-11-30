using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPOS.Models;
using StockPOS.Models.ViewModels;

namespace StockPOS.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<ActionResult<List<Product_View_Model>>> Get_ProductList_ForApp(string ProductName, string CategoryID, string ShortName)
        {
            try
            {
                var query = from product in RepositoryContext.Products
                            join category in RepositoryContext.Productcategories on product.CategoryId equals category.CategoryId
                            where
                            //(string.IsNullOrEmpty(ProductName) || product.Name == ProductName) ||
                            //(string.IsNullOrEmpty(ShortName) || product.ShortName == ShortName)
                            ((string.IsNullOrEmpty(ProductName) || EF.Functions.Like(product.Name, $"%{ProductName}%"))) ||
                            ((string.IsNullOrEmpty(ShortName) || EF.Functions.Like(product.ShortName, $"%{ShortName}%")))
                            select new Product_View_Model
                            {
                                ProductId = product.ProductId,
                                Barcode = product.Barcode,
                                Name = product.Name,
                                ShortName = product.ShortName,
                                CategoryId = product.CategoryId,
                                CategoryName = category.CategoryName,
                                BoughtPrice = product.BoughtPrice,
                                SellPriceRetail = product.SellPriceRetail,
                                SellPricewhole = product.SellPricewhole,
                                ImageUrl = product.ImageUrl
                            };


                var productListForHome = await query.ToListAsync();
                return productListForHome;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, log it, etc.
                return null;
            }
        }


        //public async Task<ActionResult<List<Product_View_Model>>> Get_ProdcutLlist_ForApp(string ProductName,string CategoryID,string ShortName)
        //{
        //    try
        //    {

        //        var query = (from prodcut in RepositoryContext.Products
        //                     join category in RepositoryContext.Productcategories on prodcut.CategoryId equals category.CategoryId
        //                     where  (string.IsNullOrEmpty(prodcut.Name) || prodcut.Name == ProductName)
        //                     && (string.IsNullOrEmpty(prodcut.ShortName) || prodcut.ShortName == ShortName)
        //                     && (string.IsNullOrEmpty(prodcut.CategoryId) || prodcut.CategoryId == CategoryID)
        //                     select new Product_View_Model
        //                     {
        //                        ProductId=prodcut.ProductId,
        //                        Barcode=prodcut.Barcode,
        //                        Name=prodcut.Name,
        //                        ShortName=prodcut.ShortName,
        //                        CategoryId=prodcut.CategoryId,
        //                        CategoryName=category.CategoryName,
        //                        BoughtPrice=prodcut.BoughtPrice,
        //                        SellPriceRetail=prodcut.SellPriceRetail,
        //                        SellPricewhole=prodcut.SellPricewhole,
        //                        ImageUrl=prodcut.ImageUrl
        //                     }
        //                    );
        //        var ProductList_For_Home = await query.ToListAsync();
        //        return ProductList_For_Home;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }


        //}


    }

}