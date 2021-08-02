import { Button, Container, Grid, TextField, Typography } from "@material-ui/core";
import React, { useState } from "react";
import style from "../Tools/Style";
import { registrarUsuario } from "../../actions/UsuarioAction";

const RegistrarUsuario = () => {

    //Obtengo los valores del formulario
    const [usuario, setUsuario] = useState({
        NombreCompleto : '',
        Email : '',
        Password : '',
        ConfirmarPassword: '',
        UserName : ''
    })

    const ingresarValorMemoria = e =>{
        const {name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    //Mando los datos a la api
    const registrar = e =>{
        e.preventDefault();
        registrarUsuario(usuario).then(response => {
            console.log('Se registro el usuario con exito', response);
            window.localStorage.setItem("token_seguridad", response.data.token);
        });
    }

    

    return(
        <Container component="main" maxWidth='md' justify='center'>
            <div style={style.paper}>
                <Typography component='h1' variant='h5'> Registro de Usuario </Typography>
                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={12}> 
                            <TextField name='NombreCompleto' value={usuario.NombreCompleto || ""} onChange={ingresarValorMemoria} variant='outlined' fullWidth label='Ingrese su nombre y apellido'/>
                        </Grid>
                        <Grid item xs={12} md={6}> 
                            <TextField name='Email' value={usuario.Email || ""} onChange={ingresarValorMemoria} type='email' variant='outlined' fullWidth label='Ingrese su email'/>
                        </Grid>
                        <Grid item xs={12} md={6}> 
                            <TextField name='UserName' value={usuario.UserName || ""} onChange={ingresarValorMemoria} variant='outlined' fullWidth label='Ingrese su username'/>
                        </Grid>
                        <Grid item xs={12} md={6}> 
                            <TextField name='Password' value={usuario.Password || ""} onChange={ingresarValorMemoria} type='password' variant='outlined' fullWidth label='Ingrese su password'/>
                        </Grid>
                        <Grid item xs={12} md={6}> 
                            <TextField name='ConfirmarPassword' value={usuario.ConfirmarPassword || ""} onChange={ingresarValorMemoria} type='password' variant='outlined' fullWidth label='Confirme su password'/>
                        </Grid>
                    </Grid>
                    <Grid container justifyContent='center'>
                        <Grid item xs={12} md={6}>
                            <Button type='submit' onClick={registrar} fullWidth variant='contained' color='primary' size='large' style={style.submit} >
                                Enviar
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>
    );
}

export default RegistrarUsuario