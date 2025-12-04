using JobSchedularDAL;
using JobSchedularDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionNodeController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public ExecutionNodeController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddExecutionNode(Models.ExecutionNode executionNode)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    ExecutionNode executionNode1 = new ExecutionNode
                    {
                        NodeId = Guid.NewGuid().ToString(),
                        NodeName = executionNode.NodeName,
                        NodeIpaddress = executionNode.NodeIpaddress,
                        NodeStatus = executionNode.NodeStatus
                    };
                    bool status = _repository.AddExecutionNode(executionNode1);
                    if (status)
                        return Ok("Execution Node added successfully");
                    else
                        return BadRequest("Failed to Add Execution Node");
                }
                else
                {
                    return BadRequest("Invalid Data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet]
        public IActionResult GetExecutionNodes()
        {
            try
            {
                var executionNodes = _repository.GetExecutionNodes();
                if (executionNodes == null || executionNodes.Count == 0)
                {
                    return NotFound("No Execution Nodes found");
                }
                return Ok(executionNodes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("nodeId/{nodeId}")]
        public IActionResult GetExecutionNodeById(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)){
                return BadRequest("Invalid Node ID");
            }
            try
            {
                var executionNode = _repository.GetExecutionNodeById(nodeId);
                if (executionNode == null)
                {
                    return NotFound("Execution Node not found");
                }
                return Ok(executionNode);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("ipAddress/{ipAddress}")]
        public IActionResult GetExecutionNodeByIpAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return BadRequest("Invalid IP Address");
            }
            try
            {
                var executionNode = _repository.GetExecutionNodeByIpAddress(ipAddress);
                if (executionNode == null)
                {
                    return NotFound("Execution Node not found");
                }
                return Ok(executionNode);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("status/{status}")]
        public IActionResult CheckExecutionNodeStatus(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                return BadRequest("Invalid Node ID");
            }
            try
            {
                var status = _repository.CheckExecutionNodeStatus(nodeId);
                if(status.IsNullOrEmpty())
                {
                    return NotFound("Execution Node not found");
                }
                return Ok(status);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateExecutionNode(Models.ExecutionNode executionNode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExecutionNode executionNode1 = new ExecutionNode
                    {
                        NodeId = executionNode.NodeId,
                        NodeName = executionNode.NodeName,
                        NodeIpaddress = executionNode.NodeIpaddress,
                        NodeStatus = executionNode.NodeStatus
                    };
                    bool status = _repository.UpdateExecutionNode(executionNode1);
                    if (status)
                        return Ok("Execution Node updated successfully");
                    else
                        return BadRequest("Failed to Update Execution Node");
                }
                else
                {
                    return BadRequest("Invalid Data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpDelete("nodeId/{nodeId}")]
        public IActionResult DeleteExecutionNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                return BadRequest("Invalid Node ID");
            }
            try
            {
                bool status = _repository.DeleteExecutionNode(nodeId);
                if (status)
                    return Ok("Execution Node deleted successfully");
                else
                    return BadRequest("Failed to Delete Execution Node");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

    }
}
