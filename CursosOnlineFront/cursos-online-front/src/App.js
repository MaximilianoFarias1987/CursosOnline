import React from 'react';
import { ThemeProvider as MuithemeProvider } from '@material-ui/core';
import theme from './theme/theme';
import RegistrarUsuario from './components/security/RegistrarUsuario';
import Login from './components/security/Login';
import PerfilUsuario from './components/security/PerfilUsuario';

function App() {
  return(
    <MuithemeProvider theme={theme}>
      <PerfilUsuario/>
    </MuithemeProvider>
  )
}

export default App;
