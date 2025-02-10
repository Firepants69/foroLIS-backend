using System.Text.Json;
using foroLIS_backend.DTOs;
using foroLIS_backend.Models;

namespace foroLIS_backend.Services
{
  

    public class GoogleService
    {
        private readonly HttpClient _httpClient;
        public GoogleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleUserDto> GetUserByToken(string token)
        {
            // Construir la URL con el token como parámetro de consulta
            Console.Write("se recibio el toke", token);
            var url = $"?access_token={token}";

            // Realizar la solicitud GET
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta JSON recibida: " + jsonResponse);

                try
                {
                    var user = JsonSerializer.Deserialize<GoogleUserDto>(jsonResponse);
                    return user;
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Error al deserializar los datos: {jsonEx.Message}");
                    throw new InvalidOperationException("Error al deserializar la respuesta de Google.", jsonEx);
                }
            }
            else
            {
                throw new HttpRequestException($"Error al obtener los datos de Google: {response.StatusCode}");
            }
        }

    }
}
