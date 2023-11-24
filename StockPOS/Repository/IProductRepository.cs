using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StockPOS.Models;
using StockPOS.Models.ViewModels;

namespace StockPOS.Repository
{
    public interface IProductRepository: IRepositoryBase<Product>
    {
        Task<ActionResult<List<Product_View_Model>>> Get_ProductList_ForApp(string ProductName, string CategoryID, string ShortName);
    }

    
}