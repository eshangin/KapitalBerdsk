using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.BuildingObjects
{
    public class SaveBuildingObjectCommand : IRequest<BuildingObject>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public BuildingObjectStatus Status { get; set; }
        public decimal Price { get; set; }
        public DateTime ContractDateStart { get; set; }
        public DateTime ContractDateEnd { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public bool IsInactive { get; set; }
    }

    public class SaveBuildingObjectCommandHandler : IRequestHandler<SaveBuildingObjectCommand, BuildingObject>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SaveBuildingObjectCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BuildingObject> Handle(SaveBuildingObjectCommand request, CancellationToken cancellationToken)
        {
            var item = request.Id.HasValue 
                ? await _mediator.Send(new GetBuildingObjectByIdQuery(request.Id.Value)) 
                : new BuildingObject();

            item.Name = request.Name;
            item.Status = request.Status;
            item.Price = request.Price;
            item.ContractDateStart = request.ContractDateStart;
            item.ContractDateEnd = request.ContractDateEnd;
            item.ResponsibleEmployeeId = request.ResponsibleEmployeeId;
            item.IsInactive = request.IsInactive;

            if (!request.Id.HasValue)
            {
                _context.BuildingObjects.Add(item);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return item;
        }
    }
}
