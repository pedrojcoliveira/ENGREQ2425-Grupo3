﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AMAPP.Web.Models;
using System.Text;

namespace AMAPP.Web.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SubscriptionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        //-------------------------------------------------------------------------------------------
        //------------------------------------ListSubscriptions--------------------------------------
        //-------------------------------------------------------------------------------------------


        // GET: ListSubscriptions
        public async Task<IActionResult> List()
        {
            try
            {
                // Fazer a requisição GET para a API
                var response = await _httpClient.GetAsync("/api/selected-product-offer");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = $"Erro ao obter as subscrições: {response.StatusCode}";
                    return View("Error");
                }

                var json = await response.Content.ReadAsStringAsync();
                var subscriptions = JsonSerializer.Deserialize<List<SubscriptionViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Passar os dados para a view
                return View("ListSubscriptions", subscriptions);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocorreu um erro ao tentar carregar as subscrições: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var requestBody = JsonSerializer.Serialize(quantity);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/selected-product-offer/{id}/quantity", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "Erro ao atualizar a quantidade.");
            return RedirectToAction("List");
        }



        //-------------------------------------------------------------------------------------------
        //-----------------------------------CreateSubscriptions-------------------------------------
        //-------------------------------------------------------------------------------------------

        // GET: CreateSubscription
        public IActionResult Create()
        {

            return View("CreateSubscriptions");
        }

        // POST: CreateSubscription
        [HttpPost]
        public async Task<IActionResult> Create(SubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/selected-product-offer", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }

                // Caso o envio da requisição falhe
                ModelState.AddModelError("", "Erro ao criar subscrição.");
            }

            return View("CreateSubscriptions", model);
        }

    }
}
