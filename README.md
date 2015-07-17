# catFinalWeb


MVC 4 	ASP.NET
Creacion de Base de Datos
Se procedera a crear las base de datos BaseDatos que contendra las 2 siguientes tablas:
1.	Categoria
2.	Producto

•	Categoria
CREATE TABLE [dbo].[Categorias] (
    [idCategoria] INT            IDENTITY (1, 1) NOT NULL,
    [nombre]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Categorias] PRIMARY KEY CLUSTERED ([idCategoria] ASC)
);


•	Producto
CREATE TABLE [dbo].[Producto] (
    [idProducto]  INT            IDENTITY (1, 1) NOT NULL,
    [nombre]      NVARCHAR (MAX) NOT NULL,
    [descripcion] NVARCHAR (MAX) NOT NULL,
    [cantidad]    INT            NOT NULL,
    [precio]      FLOAT (53)     NOT NULL,
    [picture]     IMAGE          NULL,
    [ImageMimeType]   VARCHAR (50) NULL,
    [idCategoria] INT            NOT NULL,
    CONSTRAINT [PK_dbo.Producto] PRIMARY KEY CLUSTERED ([idProducto] ASC),
    CONSTRAINT [FK_dbo.Producto_dbo.Categorias_idCategoria] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[Categorias] ([idCategoria]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_idCategoria]
    ON [dbo].[Producto]([idCategoria] ASC);



Obtendremos esto:
 
Creacion de [Entity Data Model]
En el Solucion explorer, presione clic derecho sobre la carpeta Models y luego en la ventana seleccione Ado Entity Data Model:
 

 
Creacion de Controladores
Se deberan crear dos controladores
1.	CategoriaController
 



2.	ProductoController
 
Agregar metodo a [ProductoController]
Abra el archivo y agregue el siguiente método que servira para guardar la imagen relacionada por el idProducto:
public FileContentResult GetImage(Int32 idProducto)
        {
            Producto prod = db.Producto.FirstOrDefault(c => c.idProducto == idProducto);
            if (prod != null)
            {
                string type = string.Empty;
                if (!string.IsNullOrEmpty(prod.ImageMimeType))
                {
                    type = prod.ImageMimeType;
                }
                else
                {
                    type = "image/jpeg";
                }
                return File(prod.picture, type);
            }
            else
            {
                return null;
            }
        }



Editar metodo [Create] de [ProductoController]
Abra el archivo y edite el metodo Create de la siguiente manera:

        [HttpPost]
        public ActionResult Create(Producto producto, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    producto.ImageMimeType = image.ContentType;
                    int length = image.ContentLength;
                    byte[] buffer = new byte[length];
                    image.InputStream.Read(buffer, 0, length);
                    producto.picture = buffer;
                }

                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }


Editar vista [index.cshtml] de [Producto]
Abra la vista index.cshtml ubicada en la carpeta view, agregue el siguiente código, después del td del campo descripción.

        <td>
            @if (item.picture != null)
            {
                <div style="float:left;margin-right:20px">
                    <img width="75" height="75" src="@Url.Action("GetImage", "Producto", new { item.idProducto })" />
                </div>
            }
        </td>







Editar vista [create.cshtml] de [Producto]
Abra el archivo y reemplace todo el codigo:

@model catFinal.Models.Producto

@{
    ViewBag.Title = "Create";
}

<h2>Create</h2>
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script> 
@using (Html.BeginForm("Create", "Producto", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.ValidationSummary(true)
    <fieldset>
        <legend>Producto</legend>
        <div class="editor-label">
            @Html.LabelFor(model => model.idProducto)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.idProducto)
            @Html.ValidationMessageFor(model => model.idProducto)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.nombre)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.nombre)
            @Html.ValidationMessageFor(model => model.nombre)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.descripcion)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.descripcion)
            @Html.ValidationMessageFor(model => model.descripcion)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.cantidad)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.cantidad)
            @Html.ValidationMessageFor(model => model.cantidad)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.precio)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.precio)
            @Html.ValidationMessageFor(model => model.precio)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.ImageMimeType)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.ImageMimeType)
            @Html.ValidationMessageFor(model => model.ImageMimeType)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.idCategoria, "Producto")
        </div>
        <div class="editor-field">
            @Html.DropDownList("idCategoria", String.Empty)
            @Html.ValidationMessageFor(model => model.idCategoria)
        </div>
        <div class="editor-label">
            Imagen
        </div>
        <div class="editor-field">
            <input type="file" name="image" />
        </div>
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
}
<div>
    @Html.ActionLink("Back to List", "Index")
</div>
@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
