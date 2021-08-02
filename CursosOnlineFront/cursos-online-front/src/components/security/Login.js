import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import React, { useState } from 'react';
import style from '../Tools/Style';
import LockIcon from '@material-ui/icons/Lock';
import { loginUsuario } from '../../actions/UsuarioAction';


const Login = () => {
    
    const [usuario, setUsuario] = useState({
        email : '',
        password : ''
    })

    const ingresarValorMemoria = e =>{
        const {name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }
    
    const login = e => {
        e.preventDefault();
        loginUsuario(usuario).then(response => {
            console.log('Login exitoso ', response);
            window.localStorage.setItem("token_seguridad", response.data.token);
        })
    }

    return (
        <Container maxWidth='xs'>
            <div style={style.paper}>
                <Avatar style={style.avatar}>
                <LockIcon style={style.icon}/>
                </Avatar>
                <Typography component='h1' variant='h5'>Login de usuarios</Typography>
                <form style={style.form}>
                    <TextField variant='outlined' value={usuario.email || ""} onChange={ingresarValorMemoria} label='Ingrese su email' name='email' fullWidth margin='normal'/>
                    <TextField variant='outlined' value={usuario.password || ""} onChange={ingresarValorMemoria} type='password' label='Ingrese password' name='password' fullWidth margin='normal'/>
                    <Button type='submit' onClick={login} fullWidth variant='contained' color='primary' style={style.submit}>
                        Imiciar sesión
                    </Button>
                </form>
            </div>
        </Container>
    );
};

export default Login;