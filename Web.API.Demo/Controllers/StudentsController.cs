﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.API.Demo.Dto;
using Web.API.Demo.Interfaces;
using Web.API.Demo.Models;

namespace Web.API.Demo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        public IActionResult GetAllStudent()
        {
            var students = _mapper.Map<List<StudentDto>>(_studentRepository.GetAllStudent());
            if(ModelState.IsValid)
            {
                return Ok(students);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(400)]
        public IActionResult GetStudent(int id)
        {
            if(!_studentRepository.IsExist(id))
            {
                return NotFound();
            }
            var st = _mapper.Map<StudentDto>(_studentRepository.GetStudent(id));
            if (ModelState.IsValid)
            {
                return Ok(st);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        public IActionResult GetByName(string name)
        {
            var st = _mapper.Map<StudentDto>(_studentRepository.GetByName(name));
            if(!ModelState.IsValid) return NotFound();
            return Ok(st);
        }
        [HttpGet("{IsMale}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Student>))]
        [ProducesResponseType(400)]
        public IActionResult GetByGender(bool IsMale)
        {
            var st = _mapper.Map<List<StudentDto>>(_studentRepository.GetByGender(IsMale));
            if(ModelState.IsValid)
            {
                return Ok(st);  
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(StudentDto))]
        public IActionResult CreateStudent([FromBody]StudentDto student)
        {
            //var st = _studentRepository.CreateStudent(student);
            //if (ModelState.IsValid)
            //{
            //    return Ok("Success");
            //}
            //return BadRequest(ModelState);
            // null
            // unique
            // valid
            // saved
            //if (student == null)
            //{
            //    return BadRequest(ModelState);
            //}
            //var st = _studentRepository.GetAllStudent().Where(s => s.Name.Trim().ToUpper() == student.Name.Trim().ToUpper()).FirstOrDefault();
            //if (st != null)
            //{
            //    ModelState.AddModelError("", "Already exists");
            //    return StatusCode(400, ModelState);
            //}
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //var cretedStudent = _mapper.Map<Student>(student);
            //if (!_studentRepository.CreateStudent(cretedStudent))
            //{
            //    ModelState.AddModelError("", "Smth went wrong while saving");
            //    return BadRequest(ModelState);
            //}
            //return Ok("Successfully added");
            if (student == null) return BadRequest(ModelState);
            var st = _studentRepository.GetAllStudent().Where(s => s.Name.Trim().ToUpper() == student.Name.Trim().ToUpper()).FirstOrDefault();
            if (st != null)
            {
                ModelState.AddModelError("", "Already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdStudent = _mapper.Map<Student>(student);
            if (_studentRepository.CreateStudent(createdStudent))
            {
                return Ok("Successfully created");
            }
            ModelState.AddModelError("", "Smth went wrong while saving");
            return BadRequest(ModelState);
        }
    }
}
