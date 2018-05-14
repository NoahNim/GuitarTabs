using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using GuitarTab;

namespace GuitarTab.Models
{
  public class Song
  {
    private string _songName;
    private int _artistId;
    private string _tab;
    private int _id;

    public Song(string name, int artistId, string tab, int id = 0)
    {
      _songName = name;
      _artistId = artistId;
      _tab = tab;
      _id = id;
    }
    public string GetName()
    {
      return _songName;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetArtistId()
    {
      return _artistId;
    }
    public string GetTab()
    {
      return _tab;
    }
    public override bool Equals(System.Object otherSong)
    {
      if (!(otherSong is Song))
      {
        return false;
      }
      else
      {
         Song newSong = (Song) otherSong;
         bool idEquality = this.GetId() == newSong.GetId();
         bool nameEquality = this.GetName() == newSong.GetName();
         bool artistIdEquality = this.GetArtistId() == newSong.GetArtistId();
         bool tabEquality = this.GetTab() == newSong.GetTab();
         return (idEquality && nameEquality && artistIdEquality && tabEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
    }
    public static List<Song> GetAll()
    {
      List<Song> songList = new List<Song> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM songs;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int artistId = rdr.GetInt32(2);
        string tab = rdr.GetString(3);
        Song mySong = new Song(name, artistId, tab, id);
        songList.Add(mySong);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return songList;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO songs (song_name, artist_id, tab) VALUES (@songName, @artistId, @tab);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@songName";
      name.Value = this._songName;
      cmd.Parameters.Add(name);

      MySqlParameter artistId = new MySqlParameter();
      artistId.ParameterName = "@artistId";
      artistId.Value = this._artistId;
      cmd.Parameters.Add(artistId);

      MySqlParameter tab = new MySqlParameter();
      tab.ParameterName = "@tab";
      tab.Value = this._tab;
      cmd.Parameters.Add(tab);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static void DeleteAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM songs;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}