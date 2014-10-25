using System;
using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using Newtonsoft.Json;

namespace MediaResource.Web.Controllers
{
	public class CategoryController : Controller
	{
        private readonly CategoryService _categoryService = new CategoryService();

        // GET: Category/GetChildTreeNodes
        public ActionResult GetChildNodes(ObjectType objectType, string actionName = "List")
        {
            string id = Request["id"];
            int? categoryId = String.IsNullOrEmpty(id) ? null : (int?)int.Parse(id);

            List<ZTreeNode> treeNodes = _categoryService.GetChildTreeNodesByObjectType(objectType, categoryId, actionName);
            string json = JsonConvert.SerializeObject(
                treeNodes,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Content(json);
        }

        [Obsolete]
		// GET: Category
		public JsonResult Index(ObjectType objectType)
		{
			var treeNode = _categoryService.GetTreeJsonByObjectType(objectType);
			string json = JsonConvert.SerializeObject(
				treeNode,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});

			return Json(new
			{
				data = json
			}, JsonRequestBehavior.AllowGet);
		}

        [Obsolete]
        // GET: Category/Search
        public JsonResult Search(ObjectType objectType)
        {
            var treeNode = _categoryService.GetTreeJsonByObjectType(objectType, "Category");
            string json = JsonConvert.SerializeObject(
                treeNode,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Json(new
            {
                data = json
            }, JsonRequestBehavior.AllowGet);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}