using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAPI.Dtos;
using NewsAPI.Models;
using NewsAPI.Repositories;

namespace NewsAPI.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        // I Get possono essere eseguiti anche senza l'utilizzo di autentificazione da parte
        // dell'utente

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAllNews()
        {
            var allNews = await _newsRepository.GetAll();
            return Ok(allNews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(Guid id)
        {
            var news = await _newsRepository.Get(id);
            if(news == null)
                return NotFound();
    
            return Ok(news);
        }
 
        [HttpPost]
        [Authorize]
       //[ProducesResponseType(StatusCodes.Status201Created)]
       //[ProducesResponseType(StatusCodes.Status400BadRequest)]     

        //Possibile Evolutive: inserire dei controlli per l'inserimento o aggiungere
        //una formatazione particolare (es.html)
        
        public async Task<ActionResult> CreateNews(CreateNewsDto createNewsDto)
        {       
            News news = new()
            {
                Title = createNewsDto.Title,
                Description = createNewsDto.Description,
                DateCreated = DateTime.Now
            };


            if (string.IsNullOrWhiteSpace(news.Title) || string.IsNullOrWhiteSpace(news.Description))
            {
                return BadRequest("I campi inseriti sono vuoti");
            }
            //imposto una lunghezza minima di inserimento
            if (news.Title.Length <= 5 || news.Description.Length <= 5)
            { 
                return BadRequest("I campi inseriti hanno una lunghezza minore a 5 charatteri");
            }
            //Non viene gestito il caso in cui il contenuto della new e' ugaule 
            //ad una gia' presente nel db 

            await _newsRepository.Add(news);
            return Ok("Notizia aggiunta");
        }
 
        [HttpDelete("{id}")]
        [Authorize]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]      
        public async Task<ActionResult> DeleteNews(Guid id)
        {
            //conotrollo se la news esiste
            var news = await _newsRepository.Get(id);
            if(news == null)
                return NotFound("Notizia non trovata");
            await _newsRepository.Delete(id);
            return Ok("Notizia eliminata");
        }
    
        [HttpPut("{id}")]
        [Authorize]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]     
        public async Task<ActionResult> UpdateNews(Guid id, UpdateNewsDto updateNewsDto)
        {           
            //Creo il nuovo oggetto 
            News news = new()
            {
                NewsId = id,
                Title = updateNewsDto.Title,
                Description = updateNewsDto.Description
            };

            var newsTemp = await _newsRepository.Get(id);
            if(newsTemp == null)
                return NotFound("Notizia non trovata");
            //Se la news esiste allora eseguo l'update   
            await _newsRepository.Update(news);
            return Ok("Notizia aggiornata");
        }
    }
}