using api.Repository.IRepository;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.BLOs
{

    public class IntegralPartProcessingBLO : IIntegralPartProcessingBLO, IComponentProcessingInterface
    {
        private readonly IMapper _mapper;
        private readonly IProcessRequestAndResponseRepository _processRequestAndResponseRepository;
        private readonly IUserRepository _userRepository;

        public IntegralPartProcessingBLO(IProcessRequestAndResponseRepository processRequestAndResponseRepository,
        IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _processRequestAndResponseRepository = processRequestAndResponseRepository;
            _mapper = mapper;
        }

        public async Task<ProcessResponseDto> ProcessDefectiveComponent(ProcessRequestDto processRequestDto, string username)
        {
            var userId = await _userRepository.GetUserId(username);
            var processRequest = _mapper.Map<ProcessRequest>(processRequestDto);
            ProcessResponseDto processResponseResult = new ProcessResponseDto();
            processRequest.AppUserId = userId;
            if (userId != 0)
            {
                if (processRequestDto.Id == 0)
                {
                    await _processRequestAndResponseRepository.CreateNewProcessesRequest(processRequest);
                    processResponseResult = await ComputeProcessResponse(processRequest);
                }
                else
                {
                    await _processRequestAndResponseRepository.UpdateProcessRequest(processRequest);
                    processResponseResult = await ComputeProcessResponse(processRequest);
                }
            }
            return processResponseResult;

        }

        public async Task<ProcessResponseDto> ComputeProcessResponse(ProcessRequest processRequest)
        {
            string Baseurl = "https://localhost:44335/";

            double packagingAndDeliveryCharge = 0;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(string.Format("PackagingAndDelivery/GetPackagingDeliveryCharge?" +
                    "componentType={0}&count={1}", processRequest.DefectiveComponentType, processRequest.Quantity));

                if (Res.IsSuccessStatusCode)
                {

                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    packagingAndDeliveryCharge = JsonConvert.DeserializeObject<double>(ObjResponse);

                }
            }

            ProcessResponseDto processResponse = new ProcessResponseDto();
            processResponse = new ProcessResponseDto
            {
                RequestId = processRequest.Id,
                DateOfDelivery = DateTime.Now.AddDays(2),
                ProcessingCharge = 300,
                PackagingAndDeliveryCharge = packagingAndDeliveryCharge
            };
            return processResponse;
        }

    }
}