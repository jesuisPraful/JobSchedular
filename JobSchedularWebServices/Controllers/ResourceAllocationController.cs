using JobSchedularDAL;
using JobSchedularDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceAllocationController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public ResourceAllocationController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddResourceAllocation(Models.ResourceAllocation resourceAllocation)
        {
            if (resourceAllocation == null)
            {
                return BadRequest("ResourceAllocation is null.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    ResourceAllocation resourceAllocation1 = new ResourceAllocation
                    {
                        AllocationId = resourceAllocation.AllocationId,
                        JobId = resourceAllocation.JobId,
                        ExecutionNodeId = resourceAllocation.ExecutionNodeId,
                        StartTime = resourceAllocation.StartTime,
                        EndTime = resourceAllocation.EndTime
                    };

                    var result = _repository.AddResourceAllocation(resourceAllocation1);
                    if (result)
                    {
                        return Ok("ResourceAllocation added successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "A problem happened while handling your request.");
                    }
                }
                else
                {
                    return BadRequest("Invalid model object.");
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet]
        public IActionResult GetResourceAllocations()
        {
            try
            {
                var resourceAllocations = _repository.GetResourceAllocations();
                if (resourceAllocations == null || resourceAllocations.Count == 0)
                {
                    return NotFound("No Resource Allocations Found");
                }
                return Ok(resourceAllocations);
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("jobId/{jobId}")]
        public IActionResult GetResourceAllocationsByJobId(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                return BadRequest("JobId is null or empty.");
            }

            try
            {
                var ResourceAllocations = _repository.GetResourceAllocationsByJobId(jobId);
                if (ResourceAllocations == null || ResourceAllocations.Count == 0)
                {
                    return NotFound("No Resource Allocations Found for the given JobId");
                }
                return Ok(ResourceAllocations);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("allocationId/{allocationId}")]
        public IActionResult GetResourceAllocationById(string allocationId)
        {
            if (string.IsNullOrEmpty(allocationId))
            {
                return BadRequest("AllocationId is null or empty.");
            }

            try
            {
                var resourceAllocation = _repository.GetResourceAllocationById(allocationId);
                if (resourceAllocation == null)
                {
                    return NotFound("Resource Allocation not found for the given AllocationId");
                }
                return Ok(resourceAllocation);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("executionNodeId/{executionNodeId}")]
        public IActionResult GetResourceAllocationByexecutionNodeId(string executionNodeId)
        {
            if (string.IsNullOrEmpty(executionNodeId))
            {
                return BadRequest("ExecutionNodeId is null or empty.");
            }

            try
            {
                var resourceAllocation = _repository.GetResourceAllocationByexecutionNodeId(executionNodeId);
                if (resourceAllocation == null)
                {
                    return NotFound("Resource Allocation not found for the given ExecutionNodeId");
                }
                return Ok(resourceAllocation);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPut]
        public IActionResult UpdateResourceAllocation(Models.ResourceAllocation resourceAllocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResourceAllocation resourceAllocation1 = new ResourceAllocation
                    {
                        AllocationId = resourceAllocation.AllocationId,
                        JobId = resourceAllocation.JobId,
                        ExecutionNodeId = resourceAllocation.ExecutionNodeId,
                        StartTime = resourceAllocation.StartTime,
                        EndTime = resourceAllocation.EndTime
                    };

                    var result = _repository.UpdateResourceAllocation(resourceAllocation1);
                    if (result)
                    {
                        return Ok("ResourceAllocation updated successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "A problem happened while handling your request.");
                    }
                }
                else
                {
                    return BadRequest("Invalid model object.");
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("allocationId/{allocationId}")]
        public IActionResult DeleteResourceAllocation(string allocationId)
        {
            if (string.IsNullOrEmpty(allocationId))
            {
                return BadRequest("AllocationId is null or empty.");
            }

            try
            {
                var result = _repository.DeleteResourceAllocation(allocationId);
                if (result)
                {
                    return Ok("ResourceAllocation deleted successfully.");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
