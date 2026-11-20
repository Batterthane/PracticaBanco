using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ═══════════ TEMA 1: Variables, formato y precisión ═══════════
float f = 0.1f + 0.2f;
double d = 0.1 + 0.2;
decimal m = 0.1m + 0.2m;

Console.WriteLine("=== PRECISIÓN: 0.1 + 0.2 (17 decimales) ===");
Console.WriteLine($"float   : {f:F17}");
Console.WriteLine($"double  : {d:F17}");
Console.WriteLine($"decimal : {m:F17}");
Console.WriteLine("-> float y double acumulan error; por eso el saldo usa decimal.\n");

// ═══════════ TEMA 5 crear la cuenta y validar el titular con set ═══════════
CuentaBancaria cuenta = new CuentaBancaria();
while (true)
{
    Console.Write("Nombre del titular: ");
    try
    {
        cuenta.Titular = Console.ReadLine() ?? "";
        break;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"  ✗ {ex.Message}");
    }
}

// ═══════════ TEMA 3: Ciclo con menú, switch, if/else y operadores lógicos ═══════════
bool salir = false;
while (!salir)
{
    Console.WriteLine("\n1) Depositar   2) Retirar   3) Ver saldo   4) Salir");
    Console.Write("Elige una opción: ");

    // TEMA 2: TryParse evita que el programa truene si escriben letras
    // TEMA 3: || (cualquiera de las dos condiciones inválidas) + continue
    if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 1 || opcion > 4)
    {
        Console.WriteLine("  ✗ Opción inválida.");
        continue;
    }

    switch (opcion)
    {
        case 1:
            decimal deposito = LeerMonto("Monto a depositar: ");
            cuenta.Saldo += deposito;
            Console.WriteLine($"  ✓ Depositaste RD$ {deposito:N2}");
            break;

        case 2:
            decimal retiro = LeerMonto("Monto a retirar: ");
            CalcularRetiro(retiro, out decimal comision, out decimal total);
            Console.WriteLine($"  Comisión: RD$ {comision:N2} | Total a descontar: RD$ {total:N2}");
            try
            {
                cuenta.Saldo -= total;
                Console.WriteLine("  ✓ Retiro exitoso.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✗ {ex.Message}");
            }
            break;

        case 3:
            MostrarEstado(cuenta);
            break;

        case 4:
            salir = true;
            Console.WriteLine("¡Hasta luego!");
            break;
    }
}

// ═══════════ TEMA 4: Funciones ═══════════

static decimal LeerMonto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        // TEMA 2: TryParse devuelve true/false en vez de lanzar error
        if (decimal.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint,
                             CultureInfo.InvariantCulture, out decimal monto) && monto > 0)
            return monto;

        Console.WriteLine("  ✗ Monto inválido. Usa un número mayor que 0 (ej: 250.50).");
    }
}

static void CalcularRetiro(decimal monto, out decimal comision, out decimal total)
{
    comision = monto >= 500 ? monto * 0.02m : monto * 0.01m;
    total = monto + comision;
}

// TEMA 3: if / else if / else con &&
static void MostrarEstado(CuentaBancaria c)
{
    string nivel;
    if (c.Saldo == 0) nivel = "Cuenta vacía";
    else if (c.Saldo > 0 && c.Saldo < 1000) nivel = "Saldo bajo";
    else nivel = "Saldo saludable";

    Console.WriteLine($"  Titular: {c.Titular} | Saldo: RD$ {c.Saldo:N2} ({nivel})");
}

// ═══════════ TEMA 5: Clase con propiedades get y set ═══════════
class CuentaBancaria
{
    private string _titular = "";
    private decimal _saldo = 0m;

    public string Titular
    {
        get { return _titular; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre no puede estar vacío.");
            _titular = value.Trim();
        }
    }

    public decimal Saldo
    {
        get { return _saldo; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Fondos insuficientes: el saldo no puede ser negativo.");
            _saldo = value;
        }
    }
}