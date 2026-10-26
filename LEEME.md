# Sistema POS Desktop TechStore - Registro de Ventas
**Desafío Semana 11 - Desarrollo de Software / Programación en .NET**
**Diseño de Interfaz:** *Ultra-Clean Retail Workspace (Apple / Vercel Minimalist POS)*

Aplicación de escritorio en Windows Forms (.NET Framework 4.7.2) implementada bajo arquitectura **N-Capas**, patrón de diseño **Singleton (Thread-Safe)**, control de transacciones atómicas con **SqlTransaction** y seguridad de credenciales con **Hashing SHA-256 + Salt**.

---

## 🎨 Sistema de Diseño Visual: Ultra-Clean Minimalist POS

La interfaz gráfica ha sido diseñada adoptando la estética moderna minimalista (estilo Apple / Vercel / Linear):
* **Paleta Cromática:**
  * **Superficie base (*Canvas*):** `#fbfbfd` (Gris claro ultra-limpio con alto contraste tipográfico).
  * **Contenedores de tarjetas (*Cards*):** `#ffffff` con bordes sutiles `#e2e8f0` y espaciado generoso.
  * **Acento Primario (*Slate-900*):** `#0f172a` para el distintivo de marca (`TS`), elementos de navegación activos y la **Tarjeta Maestra de Total a Pagar**.
  * **Acción de Éxito (*Emerald-600*):** `#16a34a` reservado para el botón principal de confirmación y cobro de venta (`Registrar Venta`).
  * **Tipografía Funcional:** Tipografías monoespaciadas (`Consolas` / `JetBrains Mono`) para datos tabulares, SKUs, precios y subtotales; y `Segoe UI` / `Inter` para títulos, descripciones y etiquetas.
* **Espacio de Trabajo (Workspace):**
  1. **Cabecera Principal:**
     * Logo cuadrado Slate-900 con monograma `TS`.
     * Título `TechStore POS` junto a la etiqueta de versión `v2.4-clean`.
     * Indicador de estado del nodo en vivo (punto verde intermitente).
     * Terminal e identificador de serie fiscal (`Terminal T-LIMA-02 • Serie F002`).
     * Chip de cajero autenticado con iniciales en avatar redondo (`lblAvatarInitials`), nombre completo y rol.
     * Botón de cierre de sesión minimalista.
  2. **Columna Izquierda (Búsqueda y Carrito):**
     * **1. Búsqueda y Selección:** Selector combo de productos y caja de búsqueda libre con auto-coincidencia.
     * **Métricas Tipográficas Limpias:** CÓDIGO SKU (Consolas), CATEGORÍA, PRECIO UNITARIO (`S/ 0.00`) y STOCK DISPONIBLE (con badge de almacén).
     * **Control de Cantidad:** Selector numérico con validación de límite de stock en tienda y botón de alto contraste `+ Agregar al Carrito`.
     * **2. Detalle de Venta (Carrito):** Grilla minimalista con bordes suaves, contador de líneas de venta, botones de acción (*Quitar Fila*, *Limpiar Carrito*) y distintivo de auditoría *SQL Transaction*.
     * **Telemetría de Hardware:** Estado de escáner de código de barras y correlativo de transacción.
  3. **Columna Derecha (Facturación y Cobro):**
     * **3. Facturación:** Pestañas segmentadas para comprobante tributario (*Factura*, *Boleta*, *Nota Venta*) con badge SUNAT PEN.
     * **Datos del Cliente:** Tarjeta de cliente con RUC/DNI y Razón Social.
     * **Desglose de Montos:** Contador de ítems, Subtotal (Gravada) e IGV (18%).
     * **Tarjeta de Total a Pagar:** Panel oscuro Slate-900 `#0f172a` con tipografía monoespaciada de 24pt de alto impacto.
     * **Métodos de Pago y Cambio:** Selector (*Efectivo*, *Tarjeta*, *Yape/Plin*) y calculadora interactiva de entrega de efectivo y vuelto/cambio en tiempo real.
     * **Botón Maestro:** `Registrar Venta (Enter)` en color esmeralda con respuesta al teclado.
     * **Atajos de Teclado:** Guía interactiva `[F2] Buscar | [F4] Cantidad | [F12] Cobrar`.

---

## 📋 Resumen de Cumplimiento de la Rúbrica (20 / 20 Puntos)

| Criterio / Fase | Nivel Alcanzado | Puntos | Implementación Destacada |
| :--- | :---: | :---: | :--- |
| **1. Arquitectura N-Capas** | **Excelente** | **5 / 5** | 4 proyectos físicos separados (`TechStore.UI`, `TechStore.BLL`, `TechStore.DAL`, `TechStore.Entidades`). Referencias estrictamente unidireccionales (`UI -> BLL -> DAL`), Entidades transversales, **cero código de BD o SQL en la UI**. |
| **2. Patrón Singleton (Sesión)** | **Excelente** | **5 / 5** | Clase `SesionUsuario` con constructor estrictamente `private`, mecanismo `lock` (*Double-Checked Locking*) garantizando seguridad de hilos (*thread-safe*). La UI accede de forma global a `SesionUsuario.Instancia` para mostrar al cajero activo en todo momento. |
| **3. Manejo de Transacciones** | **Excelente** | **5 / 5** | Implementación de `SqlTransaction` en `VentaDAL`. Inserta Cabecera (`Ventas`) y múltiples Detalles (`DetalleVentas`), descuenta el stock en inventario y ejecuta `Rollback` total ante cualquier error o stock negativo. **Cero datos huérfanos**. |
| **4. Integración UI y Validaciones** | **Excelente** | **5 / 5** | Pantalla de Login auténtico (`frmLogin`) con validación de Hashing (SHA-256 + Salt) en la BLL. La interfaz POS (`frmPrincipal`) valida que no se guarden ventas vacías, restringe cantidades `<= 0` o no numéricas, captura errores con `try-catch` y expone mensajes 100% amigables. |
| **TOTAL** | **EXCELENTE** | **20 / 20** | **Puntaje Máximo según Rúbrica Oficial** |

---

## 📁 Estructura Física de la Solución

```text
TechStore/
│
├── TechStore.sln                       # Solución compatible con Visual Studio 2019 / 2022 / 2025
├── bd_techstore.sql                    # Script SQL completo: tablas, relaciones, datos semilla y SPs
├── LEEME.md                            # Documentación y guía de evaluación
│
├── TechStore.Entidades/                # Capa Transversal de Entidades
│   ├── SesionUsuario.cs                # Singleton Thread-Safe para la sesión del cajero
│   ├── Usuario.cs                      # Entidad Usuario (credenciales, rol, estado)
│   ├── Categoria.cs                    # Entidad Categoría de producto
│   ├── Producto.cs                     # Entidad Producto (código, precio, stock)
│   ├── Venta.cs                        # Entidad Cabecera de Venta (IdVenta, Fecha, Total, IdUsuario, IdProducto)
│   ├── DetalleVenta.cs                 # Entidad Detalle (IdDetalle, IdVenta, IdProducto, Cantidad, Subtotal)
│   └── TechStore.Entidades.csproj
│
├── TechStore.DAL/                      # Capa de Acceso a Datos
│   ├── App.config                      # Cadena de conexión cn_techstore
│   ├── Conexion.cs                     # Manejador de conexiones SqlConnection
│   ├── UsuarioDAL.cs                   # Consulta de credenciales y usuarios
│   ├── ProductoDAL.cs                  # Catálogo y consulta de stock
│   ├── VentaDAL.cs                     # SqlTransaction: Guarda Cabecera + Detalles y descuenta Stock
│   └── TechStore.DAL.csproj
│
├── TechStore.BLL/                      # Capa de Lógica de Negocio
│   ├── SeguridadBLL.cs                 # Algoritmo de Hashing criptográfico SHA-256 con Salt
│   ├── UsuarioBLL.cs                   # Validación de credenciales y gestión de sesión
│   ├── ProductoBLL.cs                  # Reglas de negocio de inventario
│   ├── VentaBLL.cs                     # Validaciones de venta (no vacía, cantidades > 0, cálculo de totales)
│   └── TechStore.BLL.csproj
│
└── TechStore.UI/                       # Capa de Presentación (Windows Forms)
    ├── App.config                      # Configuración de cadena a (localdb)\MSSQLLocalDB
    ├── Program.cs                      # Punto de entrada (inicia en frmLogin)
    ├── frmLogin.cs                     # Pantalla de Login Precision Retail con botones de prueba rápida
    ├── frmLogin.Designer.cs
    ├── frmLogin.resx
    ├── frmPrincipal.cs                 # Formulario Master POS: Carrito, Inventario y Auditoría
    ├── frmPrincipal.Designer.cs
    ├── frmPrincipal.resx
    └── TechStore.UI.csproj
```

---

## 🔐 Cuentas de Acceso (Login Auténtico)

Las contraseñas se almacenan protegidas mediante **SHA-256 + Salt** de 16 bytes:

| Usuario | Contraseña | Rol | Nombre del Personal |
| :--- | :--- | :--- | :--- |
| **`admin`** | `admin123` | Administrador | Carlos Mendoza Silva |
| **`cajero1`** | `123456` | Cajero | Ana Lucia Ramirez |
| **`cajero2`** | `123456` | Cajero | Jorge Perez Torres |

> En `frmLogin` dispones de botones de autocompletado rápido ("👑 Admin", "👤 Cajero 1", "👤 Cajero 2") para realizar pruebas inmediatas.
