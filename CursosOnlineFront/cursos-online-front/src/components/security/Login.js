import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import React from 'react';
import style from '../Tools/Style';
import LockIcon from '@material-ui/icons/Lock';


const Login = () => {
    return (
        <Container maxWidth='xs'>
            <div style={style.paper}>
                <Avatar style={style.avatar}>
                <LockIcon style={style.icon}/>
                </Avatar>
                <Typography component='h1' variant='h5'>Login de usuarios</Typography>
                <form style={style.form}>
                    <TextField variant='outlined' label='Ingrese username' name='username' fullWidth margin='normal'/>
                    <TextField variant='outlined' type='password' label='Ingrese password' name='password' fullWidth margin='normal'/>
                    <Button type='submit' fullWidth variant='contained' color='primary' style={style.submit}>
                        Imiciar sesión
                    </Button>
                </form>
            </div>
        </Container>
    );
};

export default Login;