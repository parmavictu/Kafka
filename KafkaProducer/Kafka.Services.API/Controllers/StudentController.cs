using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kafka.Domain.Bases;
using Kafka.Domain.Commands;
using Kafka.Domain.Core;
using Kafka.Domain.Interfaces;
using Kafka.Infra.CrossCutting.VM;

namespace Kafka.Services.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class StudentController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentController(
            IMediator mediator,
            //IUser user,
            INotificationHandler<DomainNotification> notifications,
            IStudentRepository studentRepository,
            IMapper mapper)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        // GET: api/Student
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var students = _mapper.Map<IEnumerable<StudentListVM>>(await _studentRepository.GetAllAsync());
            return Response(students.ToList());
        }

        // GET: api/Student/5
        [HttpGet("{id}", Name = "Get")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var studentFound = _mapper.Map<StudentListVM>((await _studentRepository.FindAsync(student => student.Id == id)).FirstOrDefault());
            return Response(studentFound);
        }

        // POST: api/Student
        [HttpPost]
        public IActionResult Post([FromBody] StudentCreateVM studentCreateVM)
        {
            if (ModelStateValid())
            {
                return Response();
            }
            var command = _mapper.Map<StudentCreateCommand>(studentCreateVM);
            _mediator.Send(command);
            return Response("Adicionado com sucesso!");
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] StudentUpdateVM studentUpdateVM)
        {
            if (!ModelStateValid())
            {
                return Response();
            }

            var command = _mapper.Map<StudentUpdateCommand>(studentUpdateVM);
            _mediator.Send(command);
            return Response("Atualizado com sucesso!");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Response("Deletado com sucesso!");

        }
    }
}
