import {
  Avatar,
  Button,
  Drawer,
  IconButton,
  // List,
  // ListItem,
  // ListItemText,
  makeStyles,
  Toolbar,
  Typography,
} from "@material-ui/core";
import React, { useState } from "react";
import theme from "../../../theme/theme";
import FotoUsuarioTemp from "../../../logo.svg";
import { useStateValue } from "../../../contexto/store";
import { MenuIzquierda } from "./MenuIzquierda";
import { withRouter } from "react-router-dom";
import { MenuDerecha } from "./MenuDerecha";

const useStyles = makeStyles((theme) => ({
  seccionDesktop: {
    display: "none",
    [theme.breakpoints.up("md")]: {
      display: "flex",
    },
  },
  seccionMobile: {
    display: "flex",
    [theme.breakpoints.up("md")]: {
      display: "none",
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list : {
      width : 250
  },
  listItemText : {
      fontSize : '14px',
      fontWeight : 600,
      paddingLeft : '15px',
      color : '#212121'
  }
}));

const BarSesion = (props) => {
  const classes = useStyles();
  const [{ sesionUsuario }, dispatch] = useStateValue();
  const [abrirMenuIzquierda, setAbrirMenuIzquierda] = useState(false);
  const [abrirMenuDerecha, setAbrirMenuDerecha] = useState(false);

  const cerrarMenuIzquierda = () => {
    setAbrirMenuIzquierda(false);
  }

  const abrirMenuIzquierdaAccion = () => {
      setAbrirMenuIzquierda(true);
  }

  const cerrarMenuDerecha = () => {
    setAbrirMenuDerecha(false);
  }

  const abrirMenuDerechaAccion = () => {
    setAbrirMenuDerecha(true);
  }

  const salirSesionApp = () =>{
      localStorage.removeItem('token_seguridad');

      dispatch({
        type : "SALIR_SESION",
        nuevoUsuario : null,
        autenticado : false
      })
      props.history.push('/auth/login');
  }



  return (
    <React.Fragment>
        {/* Drawer para menu izquierda */}
      <Drawer
        open = {abrirMenuIzquierda}
        onClose = {cerrarMenuIzquierda}
        anchor = 'left'
      >
          <div className={classes.list} onKeyDown={cerrarMenuIzquierda} onClick={cerrarMenuIzquierda}>
            <MenuIzquierda classes={classes}/>
          </div>
      </Drawer>
      {/* Drawer para menu derecha */}
      <Drawer
        open = {abrirMenuDerecha}
        onClose = {cerrarMenuDerecha}
        anchor = 'right'
      >
          <div role='button' className={classes.list} onKeyDown={cerrarMenuDerecha} onClick={cerrarMenuDerecha}>
            <MenuDerecha 
            classes={classes} 
            salirSesion={salirSesionApp} 
            usuario={ sesionUsuario ? sesionUsuario.usuario : null}
            />
          </div>
      </Drawer>

      <Toolbar>
        <IconButton color="inherit" onClick={abrirMenuIzquierdaAccion}>
          <i className="material-icons">menu</i>
        </IconButton>
        <Typography variant="h6">Cursos Online</Typography>
        <div className={classes.grow}></div>
        <div className={classes.seccionDesktop}>
          <Button color="inherit" onClick={salirSesionApp}>Salir</Button>
          <Button color="inherit">
            {sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ""}
          </Button>
          <Avatar src={sesionUsuario.usuario.imagenPerfil || FotoUsuarioTemp}></Avatar>
        </div>

        <div className={classes.seccionMobile}>
          <IconButton color="inherit" onClick={abrirMenuDerechaAccion}>
            <i className="material-icons">more_vert</i>
          </IconButton>
        </div>
      </Toolbar>
    </React.Fragment>
  );
};

export default withRouter(BarSesion);

// Para poder utilizar las props necesitamos envolver al componente en el withRouter
