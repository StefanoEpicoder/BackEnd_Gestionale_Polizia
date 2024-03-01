using Epicode_S5_L5_BackEnd_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicode_S5_L5_BackEnd_Project.Controllers
{
    public class ViolazioneController : Controller
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        // Ottiene una violazione dal suo ID
        private Violazione GetViolazioneById(int IdViolazione)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Violazione WHERE IdViolazione = @IdViolazione";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Violazione violazione = new Violazione
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString(),
                            };
                            return violazione;
                        }
                        return null;
                    }
                }
            }
        }

        // Azione per ottenere la lista delle violazioni
        [HttpGet]
        public ActionResult ListaViolazioni()
        {
            List<Violazione> violazioni = new List<Violazione>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Violazione";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Violazione violazione = new Violazione
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString(),
                            };

                            violazioni.Add(violazione);
                        }
                    }
                }

            }
            return View(violazioni);
        }

        // Azione per aggiungere una nuova violazione
        [HttpGet]
        public ActionResult AggiungiViolazione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiViolazione(Violazione model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Violazione (Descrizione)" + "VALUES (@Descrizione)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Descrizione", model.Descrizione);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione aggiunta con successo!";
                return RedirectToAction("ListaViolazioni");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        // Azione per eliminare una violazione
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaViolazione(int IdViolazione)
        {
            Violazione violazioneDaEliminare = GetViolazioneById(IdViolazione);

            if (violazioneDaEliminare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Violazione WHERE IdViolazione = @IdViolazione";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione eliminata con successo!";
            }
            return RedirectToAction("ListaViolazioni");
        }

        // Azione per modificare una violazione
        [HttpGet]
        public ActionResult ModificaViolazione(int IdViolazione)
        {
            Violazione violazioneDaModificare = GetViolazioneById(IdViolazione);

            if (violazioneDaModificare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }

            return View(violazioneDaModificare);
        }

        [HttpPost]
        public ActionResult ModificaViolazione(Violazione violazioneModificata)
        {
            if (ModelState.IsValid)
            {
                // Query per aggiornare la descrizione di una violazione nel database
                string query =
                    "UPDATE Violazione SET " +
                    "Descrizione = @Descrizione " +
                    "WHERE IdViolazione = @IdViolazione";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        // Aggiungi i parametri al comando SQL
                        cmd.Parameters.AddWithValue("@IdViolazione", violazioneModificata.IdViolazione);
                        cmd.Parameters.AddWithValue("@Descrizione", violazioneModificata.Descrizione);

                        // Esegui l'aggiornamento nel database
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione modificata con successo!";
            }
            return RedirectToAction("ListaViolazioni");
        }


    }
}