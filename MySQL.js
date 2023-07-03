const express = require('express');
const mysql = require('mysql2');
const cors = require('cors');

const app = express();
app.use(express.json());

var allowedOrigins = ["http://localhost:3000"];
app.use(
  cors({
    origin: function (origin, callback) {
      // allow requests with no origin
      // (like mobile apps or curl requests)
      if (!origin) return callback(null, true);
      if (allowedOrigins.indexOf(origin) === -1) {
        var msg =
          "The CORS policy for this site does not " +
          "allow access from the specified Origin.";
        return callback(new Error(msg), false);
      }
      return callback(null, true);
    },
  })
);

// Configuracion de la conexion a la base de datos
const connection = mysql.createConnection({
    host:'localhost',
    user:'root',
    password:'12345678As',
    database:'garantias',
});


//ruta para listar todas las tiendas
app.get('/tiendas',(req, res)=>{
    connection.query('SELECT * FROM tiendas',(error, result)=>{
        if(error)
        {
            console.error('error al obtener las tiendas',error);
            res.status(500).json({error: 'Error al obtener las tiendas'})
        }else{
            res.json(result);
        }
    })
})

// Ruta para agregar una tienda
app.post('/tiendas',(req, res)=>{
    const { nombre, direccion, rnc, telefono, correo, estado } = req.body;
    connection.query(
      "INSERT INTO tiendas (nombre,direccion,rnc,telefono,correo,estado) VALUES (?,?,?,?,?,?)",
      [nombre, direccion, rnc, telefono, correo, estado],
      (error, result) => {
        if (error) {
          console.error("Error al agregar una tienda", error);
          res.status(500).json({ error: "Error al agregar una tienda" });
        } else {
          res.json({ id: result.insertId });
        }
      }
    );
});

// Ruta para actualizar una tienda
app.put('/tiendas/:id',(req,res)=>{
   const id = req.params.id;
  const {nombre,direccion,rnc,telefono,correo,estado} = req.body;
  connection.query(
    "UPDATE tiendas SET nombre=?,direccion=?,rnc=?,telefono=?,correo=?,estado=? WHERE id=?",
    [nombre,direccion,rnc,telefono,correo,estado,id],
    (error,result)=>{
      if(error){
        console.error("Error al actualizar la tienda",error);
        res.status(500).json({ error: "Error al actualizar la tienda" });
      }else{
        res.json({ id: result.insertId });
      }
    }
  );
});

//Ruta para eliminar tiendas
app.delete('/tiendas/:id',(req,res)=>{
  const id = req.params.id;
  connection.query(
    "DELETE FROM tiendas WHERE id=?",
    [id],
    (error,result)=>{
     if(error){
      console.error("Error al eliminar la tienda",error);
      res.status(500).json({error:"Error al eliminar la tienda"});
     }else{
      res.json({ id: result.insertId });
     }
    }
  )
});

/*************Usuarios**********************/

//ruta para listar todas los usuarios
app.get('/usuarios',(req, res)=>{
    connection.query('SELECT * FROM usuarios',(error, result)=>{
        if(error)
        {
            console.error('error al obtener los usuarios',error);
            res.status(500).json({error: 'Error al obtener los usuarios'})
        }else{
            res.json(result);
        }
    })
})

// Ruta para agregar un usuario
app.post('/usuarios',(req, res)=>{
    const { nombre, cedula, direccion, correo,telefono, clave, estado } = req.body;
    connection.query(
      "INSERT INTO usuarios (nombre, cedula, direccion, correo,telefono, clave, estado) VALUES (?,?,?,?,?,?)",
      [nombre, cedula, direccion, correo,telefono, clave, estado],
      (error, result) => {
        if (error) {
          console.error("Error al agregar una tiendaun usuario", error);
          res.status(500).json({ error: "Error al agregar un usuario" });
        } else {
          res.json({ id: result.insertId });
        }
      }
    );
});

// Ruta para actualizar un usuario
app.put('/usuarios/:id',(req,res)=>{
   const id = req.params.id;
  const { nombre, cedula, direccion, correo,telefono, clave, estado } = req.body;
  connection.query(
    "UPDATE usuarios SET nombre=?,cedula=?,direccion=?,correo=?,telefono=?,clave=?,estado=? WHERE id=?",
    [nombre, cedula, direccion, correo,telefono, clave, estado, id],
    (error, result) => {
      if (error) {
        console.error("Error al actualizar el usuario", error);
        res.status(500).json({ error: "Error al actualizar el usuario" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
});

//Ruta para eliminar usuarios
app.delete('/usuarios/:id',(req,res)=>{
  const id = req.params.id;
  connection.query("DELETE FROM usuarios WHERE id=?", [id], (error, result) => {
    if (error) {
      console.error("Error al eliminar el usuario", error);
      res.status(500).json({ error: "Error al eliminar el usuario" });
    } else {
      res.json({ id: result.insertId });
    }
  });
}); 


/*************Tipo Producto**********************/

//ruta para listar todas los Tipo Productos
app.get("/tipoproductos", (req, res) => {
  connection.query("SELECT * FROM tipo_productos", (error, result) => {
    if (error) {
      console.error("error al obtener los tipo productos", error);
      res.status(500).json({ error: "Error al obtener los tipo productos" });
    } else {
      res.json(result);
    }
  });
});

//ruta para listar todas los Tipo Productos
app.post("/filtrotipoproductos", (req, res) => {
  const {id} = req.body;
  connection.query("SELECT nombre,descripcion FROM tipo_productos WHERE id=?",[id], (error, result) => {
    if (error) {
      console.error("error al obtener los tipo productos", error);
      res.status(500).json({ error: "Error al obtener los tipo productos" });
    } else {
      res.json(result);
    }
  });
});

// Ruta para agregar un Tipo Producto
app.post("/tipoproductos", (req, res) => {
  const { nombre, descripcion } =
    req.body;
  connection.query(
    "INSERT INTO tipo_productos (nombre, descripcion) VALUES (?,?)",
    [nombre, descripcion],
    (error, result) => {
      if (error) {
        console.error("Error al agregar una tienda un tipo productos", error);
        res.status(500).json({ error: "Error al agregar un tipo productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
});

// Ruta para actualizar un Tipo Producto
app.put("/tipoproductos/:id", (req, res) => {
  const id = req.params.id;
  const { nombre, descripcion } =
    req.body;
  connection.query(
    "UPDATE tipo_productos SET nombre=?,descripcion=? WHERE id=?",
    [nombre, descripcion, id],
    (error, result) => {
      if (error) {
        console.error("Error al actualizar el tipo productos", error);
        res
          .status(500)
          .json({ error: "Error al actualizar el tipo productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
});

//Ruta para eliminar Tipo Producto
app.delete("/tipoproductos/:id", (req, res) => {
  const id = req.params.id;
  connection.query(
    "DELETE FROM tipo_productos WHERE id=?",
    [id],
    (error, result) => {
      if (error) {
        console.error("Error al eliminar el tipo productos", error);
        res.status(500).json({ error: "Error al eliminar el tipo productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
}); 


/*************Producto**********************/

//ruta para listar todas los Productos
app.get("/productos", (req, res) => {
  connection.query("SELECT * FROM productos", (error, result) => {
    if (error) {
      console.error("error al obtener los productos", error);
      res.status(500).json({ error: "Error al obtener los productos" });
    } else {
      res.json(result);
    }
  });
});

// Ruta para agregar un Producto
app.post("/productos", (req, res) => {
  const { tipo_producto_id, nombre, descripcion, precio } = req.body;
  connection.query(
    "INSERT INTO productos (tipo_producto_id, nombre, descripcion, precio) VALUES (?,?,?,?)",
    [tipo_producto_id, nombre, descripcion, precio],
    (error, result) => {
      if (error) {
        console.error("Error al agregar una tienda un productos", error);
        res.status(500).json({ error: "Error al agregar un productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
});

// Ruta para actualizar un Tipo Producto
app.put("/productos/:id", (req, res) => {
  const id = req.params.id;
  const { tipo_producto_id, nombre, descripcion, precio } = req.body;
  connection.query(
    "UPDATE productos SET tipo_producto_id=?,nombre=?,descripcion=?,precio=? WHERE id=?",
    [tipo_producto_id, nombre, descripcion, precio, id],
    (error, result) => {
      if (error) {
        console.error("Error al actualizar el productos", error);
        res.status(500).json({ error: "Error al actualizar el productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
});

//Ruta para eliminar Tipo Producto
app.delete("/productos/:id", (req, res) => {
  const id = req.params.id;
  connection.query(
    "DELETE FROM productos WHERE id=?",
    [id],
    (error, result) => {
      if (error) {
        console.error("Error al eliminar el productos", error);
        res.status(500).json({ error: "Error al eliminar el productos" });
      } else {
        res.json({ id: result.insertId });
      }
    }
  );
}); 

app.listen(8000,()=>{
    console.log('Servidor iniciado en el puerto 3000');
});