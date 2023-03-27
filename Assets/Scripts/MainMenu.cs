using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.Json;
public class InputAccount
{
    public string Name;
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loginButton;
    [SerializeField] private TMP_InputField LoginInput;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_InputField CheckInput;
    // Start is called before the first frame update
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public async void Login()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://localhost:7017/Accounts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                loginButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                loginButton.GetComponent<Image>().color = Color.red;
                Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");
            }
        }
    }
    public async void checkAvailability()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://localhost:7017/Accounts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                print(content);
                if(content == "[]")
                {
                    checkButton.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    checkButton.GetComponent<Image>().color = Color.red;
                }
                
            }
            else
            {
                checkButton.GetComponent<Image>().color = Color.red;
                Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");
            }
        }
    }
    IEnumerator SendPostRequest(string url, string postData)
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Post(url, postData);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Request sent successfully");
        }
    }

    public void sendPost()
    {
        string url = "https://example.com/api/myendpoint";
        string postData = "{\"Name\":\"John\",\"age\":30}";

        StartCoroutine(SendPostRequest(url, postData));
    }

    public async void createAccount()
    {
        using (var client = new HttpClient())
        {
            InputAccount inputAccount = new InputAccount();
            inputAccount.Name = CheckInput.text;
            var jsonData = JsonUtility.ToJson(inputAccount);
            Console.WriteLine(jsonData);
            Debug.Log(CheckInput.text);
            Debug.Log(jsonData);

            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7017/Accounts", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            Debug.Log(responseContent);
            Debug.Log(response.Content.Serialize());
            Console.ReadKey();
        }
        
    }
}
