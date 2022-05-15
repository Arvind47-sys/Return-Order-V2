using Api.DTOs;
using Api.Entities;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    interface IComponentProcessingInterface
    {
        Task<ProcessResponseDto> ComputeProcessResponse(ProcessRequest processRequest);
    }
}
