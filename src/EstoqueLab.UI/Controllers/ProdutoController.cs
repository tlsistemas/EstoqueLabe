﻿using EstoqueLab.UI.Helpers;
using EstoqueLab.UI.Models;
using EstoqueLab.Uteis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace EstoqueLab.UI.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IApiService _api;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(IApiService api,
            ILogger<ProdutoController> logger)
        {
            _api = api;
            _logger = logger;
        }


        public async Task<ActionResult> IndexAsync()
        {
            var model = new List<Produto>();
            var param = "?include=Categoria";
            var result = await _api.Get(Methods.Produto + param);
            if (!ReferenceEquals(result.Data, null))
            {
                model = JsonConvert.DeserializeObject<List<Produto>>(result.Data.ToString());
            }
            return View(model);
        }

        public async Task<ActionResult> DetailsAsync(string key)
        {
            var model = new List<Produto>();
            var param = "?include=Categoria&Key=" + key;
            var result = await _api.Get(Methods.Produto + param);
            if (!ReferenceEquals(result.Data, null))
            {
                model = JsonConvert.DeserializeObject<List<Produto>>(result.Data.ToString());
            }
            return View(model.FirstOrDefault());
        }

        public async Task<ActionResult> CreateAsync()
        {
            try
            {
                var responseConfig = await _api.Get(Methods.Categoria + "?Ativo=True");
                ViewBag.lstCategorias = JsonConvert.DeserializeObject<List<Categoria>>(responseConfig.Data.ToString());
            }
            catch (Exception)
            {
                ViewBag.lstCategorias = new List<Categoria>();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Produto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var result = await _api.Post(Methods.Produto, json);
                if (!ReferenceEquals(result.Data, null))
                {
                    TempData["sucesso_produto_lista"] = "Produto cadastrada com sucesso!";
                    return RedirectToAction("Index", "Produto");
                }
                else
                {
                    TempData["erro_produto_lista"] = "Produto não cadastrada!";
                    return RedirectToAction("Index", "Produto");
                }
            }
            catch
            {
                TempData["erro_produto_lista"] = "Produto não cadastrada!";
                return RedirectToAction("Index", "Produto");
            }
        }

        public async Task<ActionResult> EditAsync(string key)
        {
            try
            {
                var model = new List<Produto>();
                var param = "?include=Categoria&Key=" + key;
                var result = await _api.Get(Methods.Produto + param);
                if (!ReferenceEquals(result.Data, null))
                {
                    var responseConfig = await _api.Get(Methods.Categoria + "?Ativo=True");
                    ViewBag.lstCategorias = JsonConvert.DeserializeObject<List<Categoria>>(responseConfig.Data.ToString());
                    model = JsonConvert.DeserializeObject<List<Produto>>(result.Data.ToString());
                }
                return View(model.FirstOrDefault());
            }
            catch (Exception ex)
            {
                TempData["erro_produto_lista"] = "Produto não cadastrada!";
                return RedirectToAction("Index", "Produto");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Produto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var result = await _api.Put(Methods.Produto, json);
                if (!ReferenceEquals(result.Data, null))
                {
                    TempData["sucesso_produto_lista"] = "Produto atualizado com sucesso!";
                    return RedirectToAction("Index", "Produto");
                }
                else
                {
                    TempData["erro_produto_lista"] = "Produto não atualizado!";
                    return RedirectToAction("Index", "Produto");
                }
            }
            catch
            {
                TempData["erro_Produto_lista"] = "Produto não atualizado!";
                return RedirectToAction("Index", "Produto");
            }
        }

        public async Task<ActionResult> DeleteAsync(string key)
        {
            try
            {
                var param = "?Key=" + key;
                var result = await _api.Delete(Methods.Produto + param);
                if (!ReferenceEquals(result.Data, null))
                {
                    TempData["sucesso_produto_lista"] = "Produto excluida com sucesso!";
                }
                else
                {
                    TempData["erro_produto_lista"] = "Produto não excluida!";
                }
            }
            catch (Exception)
            {
                TempData["erro_produto_lista"] = "Produto não excluida!";
            }
            return RedirectToAction("Index", "Produto");
        }
    }
}
