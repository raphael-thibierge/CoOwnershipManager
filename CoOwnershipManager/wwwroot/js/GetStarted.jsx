// Hello world react component to test config

class SearchAddress extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            query: '',
            data: [],
            selectedAddress: null,
        };
    }
    
    onInputChange = e => {
        if (this.props.disabled) return;
        this.setState({
            query: e.target.value,
            selectedAddress: null,
        }, this.fetchQuerySearch);
    }

    fetchQuerySearch = () => {

        if (this.state.query === "") {
            this.setState({ data: [] });
            return;
        }

        // TODO : React Routing
        const baseUrl = '/api/Search/Addresses?query=';
        const uri = encodeURI(baseUrl + this.state.query);

        const xhr = new XMLHttpRequest();
        xhr.open('get', uri, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            
            this.setState({ data: data });
        };
        xhr.send();
        
    }

    onAddressClick = e => {
        
        const addressId = parseInt(e.target.getAttribute('data-address-id'));
        
        const selectedAddress = this.state.data.find(address => address.id = addressId);
        
        this.setState({
            query: this.addressToString(selectedAddress),
            selectedAddress: selectedAddress,
        })
    }
    
    addressToString = address => address.streetNumber+' '+address.streetName+' ' +address.zipcode+' '+address.city;

    onGoClick = e => {
        if (typeof this.props.onAddressConfirmed === 'function'){
            this.props.onAddressConfirmed(this.state.selectedAddress);
        }
    }

    render = () => {
        return (
            <div className="row mt-5">
                <div className="col-md-12">
                    <div className="input-group input-group-lg">
                        <div className="input-group-prepend">
                            <span className="input-group-text" id="inputGroup-search-adress " style={{'borderRadius': 0}}>Address</span>
                        </div>
                        <input 
                            type="text"
                            value={this.state.query}
                            onChange={this.onInputChange}
                            className={"form-control "+(this.props.disabled ? 'is-valid' : '')} style={{'borderRadius': 0}}
                            aria-label="Large" aria-describedby="inputGroup-address"
                        />
                        {this.state.selectedAddress != null && !this.props.disabled && (
                            <div className="input-group-append">
                                <button onClick={this.onGoClick} className="btn btn-outline-success" type="button" id="inputGroup-address" style={{'borderRadius': 0}}>Go !</button>
                            </div>
                        )}
                    </div>
                    <ul className="list-group col-md-10 offset-md-2">
                    {this.state.data.length > 0 && this.state.selectedAddress === null ? (
                        this.state.data.map(address => (
                            <li key={address.id} className="list-group-item list-group-item-action" data-address-id={address.id} onClick={this.onAddressClick} style={{'borderRadius': 0}}>
                                {this.addressToString(address)}
                            </li>
                        ))
                    ) : (this.state.data.length === 0 && this.state.query !== '' ) &&  (
                        <li className="list-group-item list-group-item-action disabled">No results...</li>
                    )}
                    </ul>
                </div>
            </div>
        );
    }
}

class ApartmentPicker extends React.Component {
    
    constructor(props) {
        super(props);
        this.state={
            apartment: null,
            apartments: props.apartments,
            inputValue: '',
        }
    }
    
    onInputChange = e => {
        if(this.props.selected != null) return;
        
        this.setState({
            apartment: null,
            inputValue: this.state.apartment !== null ? '' : e.target.value
        })
    }
    
    onSelect = e => {
        e.preventDefault();
        // get item id
        const id = parseInt(e.target.getAttribute('data-apartment-id'));
        // select or unselect item
        const newSelected = !(this.props.selected !== null && this.props.selected.id === id ) ?
            this.props.apartments.find(a => a.id === id) : null;
        
        this.setState({
            apartment: newSelected,
            inputValue: newSelected.description
        })
    }

    onConfirm = e => {
        
        if (this.state.apartment !== null){
            if (typeof this.props.onChange === 'function'){
                this.props.onChange(this.state.apartment);
            }
            return;
        }
        
        // TODO : React Routing
        const uri = encodeURI('/api/Apartment/');

        // TODO : display loading etc... 
        // TODO : handle errors
        const xhr = new XMLHttpRequest();
        xhr.open('post', uri, true);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhr.onload = () => {
            const apartment = JSON.parse(xhr.responseText);

            
            let apartments = this.state.apartments;
            apartments.push(apartment);
            this.setState({
                apartment: apartment,
                apartments: apartments,
            }, this.onConfirm);

        };
        xhr.send(JSON.stringify({
            BuildingId: this.props.building.id,
            Description: this.state.inputValue,
            Number: this.state.apartments.length+1,
        }));
    }
    
    render = () => {
        const selected = this.props.selected !== null ? this.props.selected.id : null;
        const placeOlder = this.state.apartments.length > 0 ?
            'Select one or name it with a little description': 'Name it with a little description for yours';
        return (
            <div className="row">
                <div className="col-md-12">
                    <div className="input-group input-group-lg">
                        <div className="input-group-prepend">
                            <span className="input-group-text" id="inputGroup-apartment " style={{'borderRadius': 0}}>Appart.</span>
                        </div>
                        <input
                            placeholder={placeOlder}
                            type="text"
                            value={this.state.inputValue}
                            onChange={this.onInputChange}
                            className={"form-control "+(this.props.selected !== null ? ' is-valid':null)} style={{'borderRadius': 0}}
                            aria-label="Large" aria-describedby="inputGroup-apartment"
                        />
                        {this.props.selected === null && this.state.inputValue !== '' && (
                            <div className="input-group-append">
                                <button onClick={this.onConfirm} className="btn btn-outline-success" type="button" id="apartmentDesciptonInput" style={{'borderRadius': 0}}>
                                    {this.state.apartment !== null ? 'Confirm !' : 'Create !'}
                                </button>
                            </div>
                        )}
                    </div>
                    {this.state.inputValue === '' && this.props.apartments.length > 0  && (
                        <ul className="list-group col-md-10 offset-md-2">
                            {this.props.apartments.map(a => (
                                <li key={a.id} data-apartment-id={a.id} onClick={this.onSelect}
                                    className={'list-group-item list-group-item-action ' + (selected === a.id ? 'active' : null)}
                                    style={{'borderRadius': 0}}
                                >
                                    {a.description}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
            </div>
        );
    }
}

class BuildingCreator extends React.Component {

    constructor(props) {
        super(props);
        this.state = {name: ''};
    }
    
    onNameChange = e => {
        this.setState({
            name: e.target.value
        });
    }
    
    onConfirm = e => {
        // TODO : React Routing
        const uri = encodeURI('/api/Building/');

        // TODO : display loading etc... 
        // TODO : handle errors
        const xhr = new XMLHttpRequest();
        xhr.open('post', uri, true);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhr.onload = () => {
            const building = JSON.parse(xhr.responseText);
            console.log("Building");
            console.log(building);
            
            if (typeof this.props.onCreate === 'function'){
                this.props.onCreate(building);
            }
        };
        xhr.send(JSON.stringify({
            AddressId: this.props.address.id,
            Name: this.state.name
        }));
    }
    
    render = () => {
        return (
            <form>
                <h3>No building found, let's create one !</h3>
                <div className="form-group mt-2">
                    <div className={'input-group input-group-lg'}>
                        <div className="input-group-prepend">
                            <span className="input-group-text" id="inputGroup-buildingNameInput" style={{'borderRadius': 0}}>Building</span>
                        </div>
                    

                        <input 
                            placeholder={'Give it a friendly name !'}
                            onChange={this.onNameChange} 
                            type="email" 
                            className="form-control" style={{'borderRadius': 0}}
                            id="buildingNameInput" 
                            aria-describedby="inputGroup-buildingNameInput" 
                        />
                        <div className="input-group-append">
                            <button onClick={this.onConfirm} className="btn btn-outline-success" type="button" id="buildingNameInput" style={{'borderRadius': 0}}>Create !</button>
                        </div>
                    </div>
                </div>
            </form>
        )
    }
}

const BuildingSelectedRender = (props) => (
    <div className="input-group input-group-lg has-success">
        <div className="input-group-prepend">
            <span className="input-group-text" id="inputGroup-building-name " style={{'borderRadius': 0}}>
                Building
            </span>
        </div>
        <input
            type="text"
            value={props.building.name}
            onChange={()=>{}} // to avoid react warning and addign readonly attribute  which disable the field, makint it grey; don't want to play with CSS now 
            className="form-control is-valid" style={{'borderRadius': 0}}
            aria-label="Large" aria-describedby="inputGroup-building-name"
        />
    </div>
)


class GetStarted extends React.Component {
    
    constructor(props) {
        super(props);
        this.state = {
            address: null,
            buildingName: '',
            building: null,
            apartments: [],
            apartment: null
        }
    }
    
    onAddressConfirmed = address => {
        this.setState({
            address: address,
            buildingName: '',
            building: null,
            appartments: [],
            apartment: null
        });

        // TODO : React Routing
        const uri = encodeURI('/api/Building?addressId='+address.id);

        // TODO : display loading
        const xhr = new XMLHttpRequest();
        xhr.open('get', uri, true);
        xhr.onload = () => {
            // todo : improve result request
            const data = JSON.parse(xhr.responseText)
            console.log(data);
            
            if (data.length > 0){
                this.setState({ building: data[0], apartment: null, apartments: data[0].apartments });
            } else {
                this.setState({ building: null, apartment:null, apartments: []});
            }
            
        };
        xhr.send();
    }
    
    onBuildingCreated = building => {
        this.setState({ building: building });
    }

    onApartmentSelected = apartment => {
        let newState = {apartment: apartment};
        if (!this.state.apartments.find(a => a.id === apartment.id)){
            newState.apartments = this.state.appartments;
            newState.apartments.push(apartment);
        }
        this.setState(newState);
    }
    
    render = () => {
        return (
            <div className={'row'}>
                <div className={'col-md-8 offset-md-2'}>
                    <div className="row">
                        <div className="col-12">
                            <SearchAddress
                                onAddressConfirmed={this.onAddressConfirmed}
                                disabled={this.state.address !== null}
                            />
                        </div>
                    </div>
                    
                    { this.state.address !== null && (
                        <div className="row">
                            <div className="col-12">
                                <hr/>
                                {this.state.building === null ? (
                                    <BuildingCreator
                                        address={this.state.address}
                                        onCreate={this.onBuildingCreated}
                                    />
                                ):(
                                    <BuildingSelectedRender building={this.state.building} />
                                )}
                            </div>
                        </div>
                    )}
                    { this.state.address !== null && this.state.building !== null && (
                        <div className="row">
                            <div className="col-12">
                                <hr/>
                                <ApartmentPicker 
                                    apartments={this.state.apartments}
                                    selected={this.state.apartment}
                                    onChange={this.onApartmentSelected}
                                    building={this.state.building}
                                />
                            </div>
                        </div>
                    )}

                    { this.state.address !== null && this.state.building !== null && this.state.apartment !== null && (
                        <div className="row ">
                            <div className="col-md-12">
                                <hr/>
                                <form id="Join" action={"/Join"} method="post">
                                    <input type="hidden" name={"ApartmentId"} value={this.state.apartment.id}/>
                                    <button type="submit" className={'btn btn-success col-md-12'}>
                                        Let's start !
                                    </button>
                                </form>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        )
    }
}