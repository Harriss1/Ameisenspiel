using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Verwantwortlich um das Objekt "World" zu instanziieren
/// World wird mit Entity-Objekten verschiedener Typen (Ant, Queen, Hive) gefüllt.
/// Das Objekt "Game" ließt die Entity-Objekte in World aus, und übergibt diese an die Grafikausgabefunktionen:
///     - UpdateDisplayContent (zum speichern des Anzeigeinhalts und Abgleich von Änderungen zum vorherigen Grafikausgabezyklus)
///     - DrawDisplayContent (verantwortlich für die Grafikausgabe)
/// </summary>

namespace Ameisenspiel {}
