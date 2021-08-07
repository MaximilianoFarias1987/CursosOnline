import React, { useEffect, useState } from 'react';
import { Grid, Snackbar, ThemeProvider as MuithemeProvider } from '@material-ui/core';
import theme from './theme/theme';
import RegistrarUsuario from './components/security/RegistrarUsuario';
import Login from './components/security/Login';
import PerfilUsuario from './components/security/PerfilUsuario';
import {BrowserRouter as Router, Switch, Route} from 'react-router-dom';
import AppNavbar from './components/navegacion/AppNavbar';
import { useStateValue } from './contexto/store';
import { obtenerUsuarioActual } from './actions/UsuarioAction';
import RutaSegura from './components/navegacion/RutaSegura';
import NuevoCurso from './components/cursos/NuevoCurso';

function App() {
  //El useStateValue me permite hacer uso de las variables globales
  const [{openSnackbar}, dispatch] = useStateValue();
  // const [{openSnackbar}, dispatch] = useStateValue();

  //Creo una variable local que la uso como bandera para saber si hay un usuario en sesion
  const[iniciaApp, setIniciaApp] = useState(false);

  useEffect(() => {
    if (!iniciaApp) {
      obtenerUsuarioActual(dispatch).then(response => {
        setIniciaApp(true);
      }).catch(error => {
        setIniciaApp(true);
      });
    }
  }, [iniciaApp]);

  //Lo que voy a hacer a continuacion es decir que cuando iniciaApp sea false que no cargue nada
  // y si es true que cargue el contenido

  return iniciaApp === false ? null : (
    <React.Fragment>
      <Snackbar 
        anchorOrigin={{vertical:'bottom', horizontal:'center'}} 
        open={openSnackbar ? openSnackbar.open : false}
        autoHideDuration={3000}
        ContentProps={{'aria-describedby' : 'message-id'}}
        message = {
          <span id='message-id'>{openSnackbar ? openSnackbar.mensaje : ""}</span>
        }
        onClose= {() => 
          dispatch({
            type : "OPEN_SNACKBAR",
            openMensaje : {
              open : false,
              mensaje : ""
            }
          })
        }
        >

      </Snackbar>
      <Router>
        <MuithemeProvider theme={theme}>
          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route
                exact
                path="/auth/registrar"
                component={RegistrarUsuario}
              />
              {/* <Route exact path="/auth/perfil" component={PerfilUsuario} /> */}

              <RutaSegura exact path="/auth/perfil" component={PerfilUsuario} />

              {/* <Route exact path="/" component={PerfilUsuario} /> */}

              <RutaSegura exact path="/" component={PerfilUsuario} />

              <RutaSegura exact path="/curso/nuevo" component={NuevoCurso} />

            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
