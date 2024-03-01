using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epicode_S5_L5_BackEnd_Project.Models;

namespace Epicode_S5_L5_BackEnd_Project.Controllers
{
    public class TrasgressoreController : Controller
    {
        // Metodo per ottenere la stringa di connessione al database
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        // Metodo per ottenere un oggetto Trasgressore dato un IdAnagrafica
        private Trasgressore GetTrasgressoreById(int IdAnagrafica)
        {
            // Apre una connessione al database utilizzando la stringa di connessione ottenuta dal metodo GetConnectionString()
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                // Apre la connessione al database
                sqlConnection.Open();
                // Query per selezionare un trasgressore dal database utilizzando l'IdAnagrafica come parametro
                string query = "SELECT * FROM Anagrafica WHERE IdAnagrafica = @IdAnagrafica";

                // Esegue il comando SQL sulla connessione aperta
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    // Aggiunge il parametro IdAnagrafica al comando SQL
                    cmd.Parameters.AddWithValue("@IdAnagrafica", IdAnagrafica);

                    // Esegue il comando SQL e ottiene un oggetto SqlDataReader per leggere i risultati
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Verifica se è stato restituito almeno un risultato
                        if (reader.Read())
                        {
                            // Crea un nuovo oggetto Trasgressore e popola i suoi attributi con i valori letti dal SqlDataReader
                            Trasgressore trasgressore = new Trasgressore
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Cap = reader["Cap"].ToString(),
                                Codice = reader["Codice"].ToString()
                            };
                            // Restituisce l'oggetto Trasgressore
                            return trasgressore;
                        }
                        // Se non è stato restituito alcun risultato, restituisce null
                        return null;
                    }
                }
            }
        }

        // Metodo per visualizzare la lista dei trasgressori
        [HttpGet]
        public ActionResult ListaTrasgressori()
        {
            List<Trasgressore> trasgressori = new List<Trasgressore>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Anagrafica";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Trasgressore trasgressore = new Trasgressore
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Cap = reader["Cap"].ToString(),
                                Codice = reader["Codice"].ToString()
                            };

                            trasgressori.Add(trasgressore);
                        }
                    }
                }

            }
            return View(trasgressori);
        }

        // Metodo per visualizzare la pagina di aggiunta di un trasgressore
        [HttpGet]
        public ActionResult AggiungiTrasgressore()
        {
            return View();
        }

        // Metodo per gestire la richiesta di aggiunta di un trasgressore
        [HttpPost]
        public ActionResult AggiungiTrasgressore(Trasgressore model)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Query per l'inserimento di un nuovo trasgressore nel database
                string query = "INSERT INTO Anagrafica (Cognome, Nome, Indirizzo, Citta, Cap, Codice)" + "VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @Cap, @Codice)";

                // Apre una connessione al database
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    // Esegue la query di inserimento dei dati del trasgressore
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", model.Citta);
                        cmd.Parameters.AddWithValue("@Cap", model.Cap);
                        cmd.Parameters.AddWithValue("@Codice", model.Codice);

                        // Esegue la query senza restituire risultati
                        cmd.ExecuteNonQuery();
                    }
                }
                // Imposta un messaggio di successo nella variabile TempData
                TempData["Messaggio"] = "Trasgressore aggiunto con successo!";
                // Reindirizza all'azione "ListaTrasgressori"
                return RedirectToAction("ListaTrasgressori");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        // Metodo per gestire la richiesta di eliminazione di un trasgressore
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaTrasgressore(int IdAnagrafica)
        {
            Trasgressore trasgressoreDaEliminare = GetTrasgressoreById(IdAnagrafica);

            if (trasgressoreDaEliminare == null)
            {
                TempData["Errore"] = "Trasgressore non trovato!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Anagrafica WHERE IdAnagrafica = @IdAnagrafica";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdAnagrafica", IdAnagrafica);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Trasgressore eliminato con successo!";
            }
            return RedirectToAction("ListaTrasgressori");
        }

        // Metodo per visualizzare la pagina di modifica di un trasgressore
        [HttpGet]
        public ActionResult ModificaTrasgressore(int IdAnagrafica)
        {
            Trasgressore trasgressoreDaModificare = GetTrasgressoreById(IdAnagrafica);

            if (trasgressoreDaModificare == null)
            {
                TempData["Errore"] = "Trasgressore non trovato!";
            }

            return View(trasgressoreDaModificare);
        }

        // Metodo per gestire la richiesta di modifica di un trasgressore
        [HttpPost]
        public ActionResult ModificaTrasgressore(Trasgressore trasgressoreModificato)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Query per aggiornare i dati del trasgressore nel database
                string query = "UPDATE Anagrafica SET " +
                    "Cognome = @Cognome, " +
                    "Nome = @Nome, " +
                    "Indirizzo = @Indirizzo, " +
                    "Citta = @Citta, " +
                    "Cap = @Cap, " +
                    "Codice = @Codice " +
                    "WHERE IdAnagrafica = @IdAnagrafica";

                // Apre una connessione al database
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    // Esegue la query di aggiornamento dei dati del trasgressore
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdAnagrafica", trasgressoreModificato.IdAnagrafica);
                        cmd.Parameters.AddWithValue("@Cognome", trasgressoreModificato.Cognome);
                        cmd.Parameters.AddWithValue("@Nome", trasgressoreModificato.Nome);
                        cmd.Parameters.AddWithValue("@Indirizzo", trasgressoreModificato.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", trasgressoreModificato.Citta);
                        cmd.Parameters.AddWithValue("@Cap", trasgressoreModificato.Cap);
                        cmd.Parameters.AddWithValue("@Codice", trasgressoreModificato.Codice);

                        // Esegue la query senza restituire risultati
                        cmd.ExecuteNonQuery();
                    }
                }
                // Imposta un messaggio di successo nella variabile TempData
                TempData["Messaggio"] = "Trasgressore modificato con successo!";
            }
            // Reindirizza all'azione "ListaTrasgressori"
            return RedirectToAction("ListaTrasgressori");
        }
    }
}