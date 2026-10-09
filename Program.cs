using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Llenado de tanque ===");

// Entradas con validación (evitan división entre cero y ciclos infinitos)
double capacidad = Entrada.LeerDouble("Capacidad máxima (L): ", 0, true);
double inicial = 0;
while (true)
{
    inicial = Entrada.LeerDouble("Volumen inicial (L): ", 0, false);
    if (inicial <= capacidad) break;
    Console.WriteLine($"  El volumen inicial no puede superar la capacidad ({capacidad} L).");
}
double porCiclo = Entrada.LeerDouble("Volumen agregado por ciclo (L): ", 0, true); // > 0 garantiza que el ciclo termina

// Objeto: tanque creado a partir de la clase Tanque
Tanque tanque = new Tanque(capacidad, inicial);

Console.WriteLine($"\nEstado inicial: {tanque.Volumen:F2} L ({tanque.PorcentajeLlenado():F1} %)");

// Ciclo while: mientras el tanque no esté lleno
int ciclo = 0;
while (!tanque.EstaLleno())
{
    ciclo++;
    tanque.Agregar(porCiclo);
    Console.WriteLine($"Ciclo {ciclo}: volumen = {tanque.Volumen:F2} L | llenado = {tanque.PorcentajeLlenado():F1} %");
}

Console.WriteLine("\nEl tanque está lleno.");

// ===== Clase: Tanque =====
class Tanque
{
    // Propiedades (double, en litros)
    public double Capacidad { get; }
    public double Volumen { get; private set; }

    public Tanque(double capacidad, double volumenInicial)
    {
        Capacidad = capacidad;
        Volumen = Math.Min(volumenInicial, capacidad);
    }

    // Método: agrega líquido sin exceder la capacidad (L)
    public void Agregar(double litros) => Volumen = Math.Min(Volumen + litros, Capacidad);

    // Método: indica si el tanque está lleno
    public bool EstaLleno() => Volumen >= Capacidad;

    // Método: porcentaje de llenado (%), protegido contra división entre cero
    public double PorcentajeLlenado() => Capacidad > 0 ? Volumen / Capacidad * 100.0 : 0.0;
}

// ===== Clase auxiliar: lectura segura de datos =====
static class Entrada
{
    // Lee un número decimal; repite mientras el dato no sea válido o no cumpla el mínimo
    public static double LeerDouble(string mensaje, double minimo, bool estricto)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? texto = Console.ReadLine();
            if (double.TryParse((texto ?? "").Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double v)
                && (estricto ? v > minimo : v >= minimo))
                return v;
            Console.WriteLine(estricto ? $"  Valor inválido: debe ser un número mayor que {minimo}."
                                       : $"  Valor inválido: debe ser un número mayor o igual que {minimo}.");
        }
    }

    // Lee un entero dentro de un rango; repite mientras sea inválido
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine()?.Trim(), out int v) && v >= minimo && v <= maximo)
                return v;
            Console.WriteLine($"  Valor inválido: debe ser un entero entre {minimo} y {maximo}.");
        }
    }

    // Lee texto no vacío
    public static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? t = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(t)) return t;
            Console.WriteLine("  El texto no puede estar vacío.");
        }
    }
}
