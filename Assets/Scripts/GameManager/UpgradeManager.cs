using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public List<PlayerUpgrade> mejorasDisponibles;

    public GameObject panelOpciones; // Activar al subir de nivel
    public Button[] botones; // 3 botones
    public TextMeshProUGUI[] textosNombre;
    public TextMeshProUGUI[] textosDescripcion;

    public PlayerStatsManager statsManager;

    private List<PlayerUpgrade> opcionesActuales;

    public void MostrarOpcionesMejora(int nivelJugador)
    {
        opcionesActuales = ObtenerMejorasAleatorias(nivelJugador);

        for (int i = 0; i < botones.Length; i++)
        {
            var mejora = opcionesActuales[i];
            textosNombre[i].text = mejora.nombre;
            textosDescripcion[i].text = mejora.descripcion;

            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() =>
            {
                statsManager.AplicarMejora(mejora);
                panelOpciones.SetActive(false);
                Time.timeScale = 1f;
            });
        }

        panelOpciones.SetActive(true);
    }

    private List<PlayerUpgrade> ObtenerMejorasAleatorias(int nivel)
    {
        float probComun = Mathf.Clamp01(1f - nivel * 0.02f);
        float probRara = Mathf.Clamp01(0.25f + nivel * 0.015f);
        float probEpica = Mathf.Clamp01(0.05f + nivel * 0.01f);

        List<PlayerUpgrade> seleccionadas = new List<PlayerUpgrade>();
        System.Random rng = new System.Random();

        while (seleccionadas.Count < 3)
        {
            PlayerUpgrade candidata = mejorasDisponibles[rng.Next(mejorasDisponibles.Count)];

            float rand = (float)rng.NextDouble();
            bool entra = false;

            switch (candidata.rareza)
            {
                case Rareza.Comun: entra = rand < probComun; break;
                case Rareza.Rara: entra = rand < probRara; break;
                case Rareza.Epica: entra = rand < probEpica; break;
            }

            if (entra && !seleccionadas.Contains(candidata))
                seleccionadas.Add(candidata);
        }

        return seleccionadas;
    }
}
