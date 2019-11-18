﻿using _14_CRUD_PersonasUWP_BL;
using _14_CRUD_PersonasUWP_Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _14_CRUD_PersonasUWP_UI
{
    public class modeloVista : clsVMBase
    {

        private ObservableCollection<clsPersona> _listadoPersona;
        private ObservableCollection<clsPersona> _listadoPersonaFiltrada;
        private ObservableCollection<clsDepartamento> _listadoDepartamentos;
        private clsPersona _personaSeleccionada;
        private DelegateCommand _EliminarCommand;
        private DelegateCommand _BuscarCommand;
        private DelegateCommand _InsertarCommand;
        private DelegateCommand _GuardarCommand;
        private String _textoABuscar;
        

        //constructor por defecto
        public modeloVista() {
            //rellenamos el constructor con el listado de personas
            clsListadoPersonasBL listPersonas = new clsListadoPersonasBL();
            clsListadoDepartamentosBL listDepartamentos = new clsListadoDepartamentosBL();
            _listadoPersona = new ObservableCollection<clsPersona>(listPersonas.listadoPersonas_BL());
            _listadoPersonaFiltrada = new ObservableCollection<clsPersona>(listPersonas.listadoPersonas_BL());
            _listadoDepartamentos = new ObservableCollection<clsDepartamento>(listDepartamentos.listadoDepartamentos());
        }

        public clsPersona personaSeleccionada {
            get { return _personaSeleccionada; }
            set {
                if (_personaSeleccionada != value)
                {
                    _personaSeleccionada = value;
                    _EliminarCommand.RaiseCanExecuteChanged();
                    _GuardarCommand.RaiseCanExecuteChanged();
                    _GuardarCommand.CanExecute(personaSeleccionada);
                    NotifyPropertyChanged("personaSeleccionada");
                }
            }
        }

        public ObservableCollection<clsPersona> listadoPersona {
            get { return _listadoPersona; }
            set { _listadoPersona = value; }
        }

        public ObservableCollection<clsDepartamento> listadoDepartamentos
        {
            get { return _listadoDepartamentos; }
            set { _listadoDepartamentos = value; }
        }
        public ObservableCollection<clsPersona> listadoPersonaFiltrada
        {
            get {return _listadoPersonaFiltrada;}
            set{ _listadoPersonaFiltrada = value;}
        }

        public DelegateCommand EliminarCommand { 
            get {
                
                   _EliminarCommand = new DelegateCommand(EliminarCommand_Executed, EliminarCommand_CanExecuted);
                    return _EliminarCommand;
                } 
        }

        public DelegateCommand GuardarCommand
        {
            get {
                _GuardarCommand = new DelegateCommand(GuardarCommand_Executed,GuardarCommand_CanExecuted);
                return _GuardarCommand;
            }

        }
        public DelegateCommand InsertarCommand {
            get {
                _InsertarCommand = new DelegateCommand(InsertarCommand_Executed);
                return _InsertarCommand;

            }
        }

        public String textoABuscar
        {

            get { return _textoABuscar; }

            set
            {
                _textoABuscar = value;
                if (!String.IsNullOrEmpty(_textoABuscar))
                {                   
                    _BuscarCommand.Execute(null);
                    _BuscarCommand.RaiseCanExecuteChanged();
                }
                else {
                    _listadoPersonaFiltrada = _listadoPersona;
                }
                NotifyPropertyChanged("listadoPersonaFiltrada");
            }
        }

        private async void GuardarCommand_Executed() {
            int filasAfectadas;
            gestionadoraPersonas_BL gestionadora = new gestionadoraPersonas_BL();
            clsListadoPersonasBL listadoPersonasBL = new clsListadoPersonasBL();
            ContentDialog confirmadoCorrectamente = new ContentDialog();

                if (listadoPersonasBL.existePersona_BL(_personaSeleccionada.idPersona))
                {
                    filasAfectadas = gestionadora.editarPersona_BL(_personaSeleccionada);
                    if (filasAfectadas == 1)
                    {
                        actualizar();
                        confirmadoCorrectamente.Title = "Guardado";
                        confirmadoCorrectamente.Content = "Se ha guardado correctamente";
                        confirmadoCorrectamente.PrimaryButtonText = "Aceptar";
                        ContentDialogResult resultado = await confirmadoCorrectamente.ShowAsync();
                    }
                }
                else
                {
                    filasAfectadas = gestionadora.insertarPersona_BL(_personaSeleccionada);
                    if (filasAfectadas == 1)
                    {
                        actualizar();
                        confirmadoCorrectamente.Title = "Guardado";
                        confirmadoCorrectamente.Content = "Se ha insertado correctamente";
                        confirmadoCorrectamente.PrimaryButtonText = "Aceptar";
                        ContentDialogResult resultado = await confirmadoCorrectamente.ShowAsync();
                    }
                }
        }
        

        private bool GuardarCommand_CanExecuted() {
            bool guardable = false;

            if (personaSeleccionada != null)
            {
                guardable = true;
            }

            return guardable;
        }

        private void InsertarCommand_Executed()
        {
            _personaSeleccionada = new clsPersona();
            NotifyPropertyChanged("personaSeleccionada");
            NotifyPropertyChanged("EliminarCommand");
        }

        private async void EliminarCommand_Executed()
        {
            int filasAfectadas = 0;
            gestionadoraPersonas_BL gestionadora = new gestionadoraPersonas_BL();
            clsListadoPersonasBL listPersonas = new clsListadoPersonasBL();

            ContentDialog eliminadoCorrectamente = new ContentDialog();
            ContentDialog confirmacion = new ContentDialog();

            confirmacion.Title = "Eliminar";
            confirmacion.Content = "¿Seguro que quieres borrar?";
            confirmacion.PrimaryButtonText = "Cancelar";
            confirmacion.SecondaryButtonText = "Aceptar";
            ContentDialogResult resultado = await confirmacion.ShowAsync();

            if (resultado == ContentDialogResult.Secondary)
            {
                filasAfectadas = gestionadora.borrarPersona_BL(_personaSeleccionada.idPersona);
                if (filasAfectadas == 1)
                {
                    actualizar();

                    eliminadoCorrectamente.Title = "Guardado";
                    eliminadoCorrectamente.Content = "Se ha eliminado correctamente";
                    eliminadoCorrectamente.PrimaryButtonText = "Aceptar";
                    ContentDialogResult resultadoEliminado = await eliminadoCorrectamente.ShowAsync();
                }
            }
        }

        private bool EliminarCommand_CanExecuted()
        {
            bool eliminable = false;

            if (personaSeleccionada != null)
            {
                eliminable = true;
            }

            return eliminable;
        }

        public DelegateCommand BuscarCommand
        {

            get
            {
                _BuscarCommand  = new DelegateCommand(BuscarCommand_Executed,BuscarCommand_CanExecuted);
                return _BuscarCommand;
            }
        }
        private void BuscarCommand_Executed()
        {
            filtrar(); 
        }

        private bool BuscarCommand_CanExecuted() {
            bool modificable = false;

            if (!String.IsNullOrEmpty(_textoABuscar)) {
                modificable = true; 
            }

            return modificable;
        }

        public void filtrar()
        {
            ObservableCollection<clsPersona> listadoPersonasFiltradas = new ObservableCollection<clsPersona>(_listadoPersona.ToList().FindAll(persona => String.Concat(persona.nombre, " ", persona.apellidos).Contains(_textoABuscar)));

           _listadoPersonaFiltrada = listadoPersonasFiltradas;
        }

        public void actualizar() {
            clsListadoPersonasBL listPersonas = new clsListadoPersonasBL();
            _listadoPersonaFiltrada = new ObservableCollection<clsPersona>(listPersonas.listadoPersonas_BL());
            NotifyPropertyChanged("listadoPersonaFiltrada");
        }

    }
}
