using System;
using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using Newtonsoft.Json;

namespace MediaResource.Web.Controllers
{
    [Authorize]
    public class NodeController : Controller
    {
        private readonly NodeService _nodeService = new NodeService();

        // GET: Node
        public ActionResult Index(int? topicId, int? nodeId)
        {
            ViewBag.TopicId = topicId;
            ViewBag.NodeId = nodeId;

            return View();
        }

        // GET: Node/GetChildNodes
        public ActionResult GetChildNodes(int topicId)
        {
            string id = Request["id"];
            int? nodeId = String.IsNullOrEmpty(id) ? null : (int?)int.Parse(id);

            List<ZTreeNode> treeNodes = _nodeService.GetChildTreeNodesByTopic(topicId, nodeId);
            string json = JsonConvert.SerializeObject(
                treeNodes,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Content(json);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nodeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}