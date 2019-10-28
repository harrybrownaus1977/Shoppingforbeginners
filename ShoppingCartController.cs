using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SanaCommerce.Models;

namespace SanaCommerce.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ISMEntities context = new ISMEntities();

        // GET: ShoppingCart
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                ShoppingCartViewModel oModel = new ShoppingCartViewModel((List<ItemModel>)Session["itemList"], (TotalModel)Session["totModel"]);
                return View(oModel);
            }
            else
            {
                List<ItemModel> itemListModels = new List<ItemModel>();
                List<ItemModel> existItemLists = new List<ItemModel>();
                TotalModel existTotal = new TotalModel();
                TotalModel oTotal = new TotalModel();
                //Session["itemList"] = null;
                //Session["totModel"] = null;
                if (Session["itemList"] == null)
                {

                    var oProduct = context.tblProducts.Where(x => x.ProductID == id).FirstOrDefault();
                    ItemModel oItem = new ItemModel();
                    oItem.ProductID = oProduct.ProductID;
                    oItem.Name = oProduct.Name;
                    oItem.Qty = 1;
                    oItem.Rate = oProduct.Price.Value;
                    oItem.ItemTotal = oProduct.Price.Value;
                    itemListModels.Add(oItem);

                    oTotal.Total = oItem.Rate * 1;

                    Session["itemList"] = itemListModels;
                    Session["totModel"] = oTotal;
                }
                else
                {
                    existItemLists = (List<ItemModel>)Session["itemList"];
                    existTotal = (TotalModel)Session["totModel"];
                    var oProductExist = existItemLists.Where(x => x.ProductID == id).FirstOrDefault();                    

                    decimal totalVal = existTotal.Total;
                   // bool isNewItem = false;

                    if (oProductExist != null)
                    {
                        foreach (var item in existItemLists)
                        {
                            if (item.ProductID == oProductExist.ProductID)
                            {
                                item.Qty = item.Qty + 1;
                                totalVal = totalVal + (1 * item.Rate);
                            }
                        }
                    }
                    else
                    {
                        var oProduct = context.tblProducts.Where(x => x.ProductID == id).FirstOrDefault();
                        ItemModel oItem = new ItemModel();
                        oItem.ProductID = oProduct.ProductID;
                        oItem.Name = oProduct.Name;
                        oItem.Qty = 1;
                        oItem.Rate = oProduct.Price.Value;
                        oItem.ItemTotal = oProduct.Price.Value;
                        totalVal = totalVal + (oItem.Qty * oItem.Rate);
                        existItemLists.Add(oItem);
                    }

                    
                    //if (isNewItem)
                    //{
                        
                    //}
                    oTotal.Total = totalVal;
                    Session["itemList"] = existItemLists;
                    Session["totModel"] = oTotal;
                }

                ShoppingCartViewModel oModel = new ShoppingCartViewModel((List<ItemModel>)Session["itemList"], (TotalModel)Session["totModel"]);

                return View(oModel);
            }
            
        }



        // GET: ShoppingCart/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }          
                TotalModel oTotal = new TotalModel();
                List<ItemModel> existItemLists = (List<ItemModel>)Session["itemList"];

                var oProduct = existItemLists.Single(x => x.ProductID == id);
                if (oProduct == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    existItemLists.Remove(oProduct);
                    decimal totalVal = 0;
                    foreach (var item in existItemLists)
                    {
                        totalVal = totalVal + (item.Qty * item.Rate);
                    }

                    oTotal.Total = totalVal;
                    Session["itemList"] = existItemLists;
                    Session["totModel"] = oTotal;
                }               
                
            ShoppingCartViewModel oModel = new ShoppingCartViewModel((List<ItemModel>)Session["itemList"], (TotalModel)Session["totModel"]);
            return RedirectToAction("Index", "ShoppingCart", oModel);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
