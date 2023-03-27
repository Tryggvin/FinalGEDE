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
[Serializable]
public class Account
{
    public string id;
    public string name;
    public string invId;
}
[Serializable]
public class Inventory
{
    public int id;
    public List<int> itemIds;
    public int accountId;
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loginButton;
    [SerializeField] private TMP_InputField LoginInput;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_InputField CheckInput;
    public Account[] accountList;
    private Account currentAccount;
    private Inventory currentInventory;
    
    // Start is called before the first frame update
    
    public void PlayGame()
    {
        GameManager.instance.currentInventory = currentInventory;
        GameManager.instance.currentAccount = currentAccount; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public async void Login()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("http://localhost:5224/Accounts");
            bool isAvailableName = true;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var newContent = "{\"Items\":" + content + "}";
                //Debug.Log(newContent);
                
                accountList = JsonHelper.FromJson<Account>(newContent);
                // Debug.Log(accountList);
                foreach (Account acc in accountList)
                {
                    //Debug.Log("this is the name: " + acc.name);
                    if(acc.name == LoginInput.text)
                    {
                        isAvailableName = false;
                        currentAccount = acc;
                        Debug.Log(currentAccount.name);
                        GetInventory();
                    }
                }
                if (isAvailableName)
                {
                    loginButton.GetComponent<Image>().color = Color.red;
                }
                else
                {
                    loginButton.GetComponent<Image>().color = Color.green;
                }
                
                
            }
            else
            {
                loginButton.GetComponent<Image>().color = Color.red;
                Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");
            }
        }
    }
    public async void GetInventory()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("http://localhost:5224/Inventories/" + currentAccount.invId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //var newContent = "{\"Items\":" + content + "}";
                Debug.Log(content);

                currentInventory = JsonUtility.FromJson<Inventory>(content);
                Debug.Log("letsgo");
                Debug.Log(currentInventory);
            }
            else
            {
                Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");
            }
        }
    }

    public async void checkAvailability()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("http://localhost:5224/Accounts");

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

            var response = await client.PostAsync("http://localhost:5224/Accounts", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            Debug.Log(responseContent);
            List<Account> accounts = JsonUtility.FromJson<List<Account>>(responseContent);
            
            Debug.Log(accounts);
            Console.ReadKey();
        }
        
    }
}
