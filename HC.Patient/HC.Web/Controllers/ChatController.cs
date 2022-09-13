using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Chat")]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly IChatHubService _chatHubService;
        private readonly ITokenService _tokenService;
        IHubContext<ChatHub> _chatHubContext;

        private readonly IChatRoomUserRepository _chatRoomUserRepository;

        public ChatController(
            IChatService chatService, 
            IChatHubService chatHubService, 
            ITokenService tokenService, 
            IHubContext<ChatHub> chatHubContext,
            IChatRoomUserRepository chatRoomUserRepository)
        {
            _chatService = chatService;
            _chatHubService = chatHubService;
            _tokenService = tokenService;
            _chatHubContext = chatHubContext;
            _chatRoomUserRepository = chatRoomUserRepository;
        }


        [HttpGet]
        [Route("GetChatHistory")]
        public JsonResult GetChatHistory(ChatParmModel chatParmModel)
        {
            return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetChatHistory(chatParmModel, GetToken(HttpContext))));
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("SendImage/{userId}/{roomId}")]
        public async Task<JsonResult> SendImage(int userId, int roomId)
        {
            var jsonModel = new JsonModel();
            TokenModel tokenModel = GetBussinessToken(HttpContext, _tokenService);
            jsonModel = await _chatService.ExecuteFunctions<Task<JsonModel>>(() => _chatService.SendFileInChat(Request.Form.Files, roomId, userId,tokenModel ));
            if (jsonModel.StatusCode == (int)HttpStatusCode.OK)
            {
                string connectionId = _chatService.GetConnectionId(userId);
                
                if (connectionId != null)
                {
                    var connectionIds = _chatRoomUserRepository.GetConectionIdInParticularRoom<RoomConnectionModel>(roomId,userId, tokenModel);
                    if (connectionIds != null && connectionIds.ToList().Count > 0)
                    {
                        connectionIds.ToList().ForEach(async x =>
                        {
                            await _chatHubContext.Clients.Client(x.ConnectionId).SendAsync("ReceiveMessage", jsonModel.data, userId, roomId);
                        });

                        //await Clients.All.SendAsync("ReceiveMessage", chatFileResponseModels, UserId, currentRoomId);
                    }
                    //await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", jsonModel.data, userId, roomId);
                }
            }
            return Json(jsonModel);

            ////var formFile = HttpContext.Request.Form.Files;
            //var files = Request.Form.Files;
            ////var folderName = Path.Combine("Chat", "Files");
            ////var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            //if (files.Any(f => f.Length == 0))
            //    return new JsonModel(0, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);

            //foreach (var file in files)
            //{
            //    //return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetChatHistory(chatParmModel, GetToken(HttpContext))));
            //    var jsonModel = _chatService.SendFileInChat(file, roomId, userId);
            //    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //    //var fullPath = Path.Combine(pathToSave, fileName);
            //    //var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

            //    //using (var stream = new FileStream(fullPath, FileMode.Create))
            //    //{
            //    //    file.CopyTo(stream);
            //    //}
            //}

            ////return Ok("All the files are successfully uploaded.");

            ////if (HttpContext.Request.Form.Files.Count>0)
            ////{

            ////}
            ////return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetChatHistory(chatParmModel, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("chat")]
        public JsonResult Chat(string name, string message)
        {
            _chatHubService.SendToAll(name, message);
            return null;
        }

        [HttpGet]
        [Route("GetCareChatHistory")]
        public JsonResult GetCareChatHistory(ChatParmModel chatParmModel)
        {
            return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetCareChatHistory(chatParmModel, GetToken(HttpContext))));
        }

    }
}