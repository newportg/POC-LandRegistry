//using AutoMapper;
//using FluentValidation;
//using KnightFrank.Hub.LandRegistry.Common.Models;
//using KnightFrank.Hub.LandRegistry.Common.Query;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KnightFrank.Hub.LandRegistry.Common.Handlers
//{
//    public class FindRequestQueryHandler : IRequestHandler<FindProperty, FindResponse>
//    {
//        private readonly ILandRegistrySvc _landRegistry;
//        private readonly ILandRegistryTable _landRegistryTable;
//        private readonly IMapper _mapper;
//        private readonly ILogger _logger;
//        private readonly IValidator<FindResponse> _validator;

//        public FindRequestQueryHandler(ILandRegistrySvc lrSvc, ILandRegistryTable lrTable, IMapper mapper, ILogger<FindRequestQueryHandler> logger, IValidator<FindResponse> validator)
//        {
//            _landRegistry = lrSvc;
//            _landRegistryTable = lrTable;
//            _mapper = mapper;
//            _logger = logger;
//            _validator = validator;
//        }

//        public Task<FindResponse> Handle(FindProperty request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("Find Request Query Handler");

//            // Register property in table - Status new
//            var dto = _mapper.Map<LandRegistryDto>(request);
//            var sta = _landRegistryTable.Upsert(dto);

//            // Land Registry Find Property ? Should the service return a dto ?
//            var lrres = _landRegistry.FindProperty(dto);

//            // Update property in table - Status requested
//            var res = _mapper.Map<FindResponse>(request);
//            res.Status = sta.ToString();

//            //_ = _validator.Validate(res);
//            return Task.FromResult(res);
//        }
//    }
//}
