﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WhatToWear.Weather
{
  public class Core
  {
    public static async Task<Weather> GetWeather(Query queryWeather)
    {
      string queryWeatherString = "http://api.openweathermap.org/data/2.5/weather?q="
                                  + queryWeather.City + ',' + queryWeather.Country;
      switch (queryWeather.Format)
      {
        case "metric":
        case "imperial":
          queryWeatherString += "&units=" + queryWeather.Format;
          break;
      }
      queryWeatherString += "&appid=" + Constants.Key;

      var results = await DataService.getDataFromService(queryWeatherString).ConfigureAwait(false);

      if (results["weather"] != null)
      {
        Debug.WriteLine(results["weather"]);
        Weather weather = new Weather();
        weather.Title = (string)results["name"];
        weather.Temperature = (string)results["main"]["temp"];
        switch (queryWeather.Format)
        {
          case "metric":
            weather.Temperature += " C";
            break;
          case "imperial":
            weather.Temperature += " F";
            break;
          default:
            weather.Temperature += " K";
            break;
        }
        weather.Wind = (string)results["wind"]["speed"] + " mph";
        weather.Humidity = (string)results["main"]["humidity"] + " %";
        weather.Visibility = (string)results["weather"][0]["main"];
        weather.Icon = (string)results["weather"][0]["icon"];
        weather.DisplayIcon = "http://openweathermap.org/img/w/" + weather.Icon + ".png";

        DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
        DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
        weather.Sunrise = sunrise.ToString() + " UTC";
        weather.Sunset = sunset.ToString() + " UTC";
        return weather;
      }
      return null;
    }
  }
}