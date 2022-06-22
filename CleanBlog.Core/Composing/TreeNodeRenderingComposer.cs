using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Web.Trees;

namespace CleanBlog.Core.Composing
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class TreeNodeRenderingComposer : ComponentComposer<TreeNodeRenderingComponent>, IUserComposer
    { }

    public class TreeNodeRenderingComponent : IComponent
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IMemberTypeService _memberTypeService;

        public TreeNodeRenderingComponent(IContentTypeService contentTypeService,
            IMediaTypeService mediaTypeService, IMemberTypeService memberTypeService)
        {
            _contentTypeService = contentTypeService;
            _mediaTypeService = mediaTypeService;
            _memberTypeService = memberTypeService;
        }

        public void Initialize()
        {
            TreeControllerBase.TreeNodesRendering += TreeControllerBase_TreeNodesRendering;
        }

        public void Terminate()
        { }

        /// <summary>
        /// Checks the tree alias and node alias to see if we should update the icon
        /// Gets the types from the type services and calls the method to update the node icons
        /// </summary>
        /// <param name="sender">Tree controller base</param>
        /// <param name="e">Event args</param>
        private void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            switch (sender.TreeAlias)
            {
                case "documentTypes":
                    var contentTypeIcons = _contentTypeService.GetAll().ToDictionary(c => c.Id, c => c.Icon);
                    UpdateNodeIcons(e, contentTypeIcons, "documentTypes");
                    break;
                case "memberTypes":
                    var memberTypeIcons = _memberTypeService.GetAll().ToDictionary(c => c.Id, c => c.Icon);
                    UpdateNodeIcons(e, memberTypeIcons, "memberTypes");
                    break;
                case "mediaTypes":
                    var mediaTypeIcons = _mediaTypeService.GetAll().ToDictionary(c => c.Id, c => c.Icon);
                    UpdateNodeIcons(e, mediaTypeIcons, "mediaTypes");
                    break;
                default:
                    // don't change the icon
                    break;
            }
        }

        /// <summary>
        /// Loops through the nodes and updates the icon to the one from the type service
        /// </summary>
        /// <param name="e">Event args</param>
        /// <param name="nodeIcons">Dictionary of node icons</param>
        /// <param name="nodeTypeAlias">The node type alias we are interested in</param>
        private static void UpdateNodeIcons(TreeNodesRenderingEventArgs e, Dictionary<int, string> nodeIcons, string nodeTypeAlias)
        {
            foreach (var node in e.Nodes)
            {
                if (node.NodeType == nodeTypeAlias)
                {
                    if (int.TryParse(node.Id.ToString(), out var nodeId) && nodeId > 0)
                    {
                        node.Icon = nodeIcons[nodeId];
                    }
                }
            }
        }
    }
}