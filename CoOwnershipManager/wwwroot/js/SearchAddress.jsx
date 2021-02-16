// Hello world react component to test config

class SearchAddress extends React.Component {

    constructor(props) {
        super(props)
        this.state = this.getInitialState();
    }

    getInitialState() {
        return {
            query: '',
            data: []
        };
    }

    onInputChange = e => {
        console.log("change");
        console.log(e.target.value);
        console.log(this);
        this.setState({
            query: e.target.value
        }, this.fetchQuerySearch);
    }

    fetchQuerySearch = () => {

        // TODO : React Routing
        const baseUrl = '/api/Search/Addresses?query=';
        const uri = encodeURI(baseUrl + this.state.query);

        const xhr = new XMLHttpRequest();
        xhr.open('get', uri, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            this.setState({ data: data });
        };
        xhr.send();
        
    }

    render = () => {
        return (
            <div className="row">
                <div className="col-md-6 offset-md-3">


                    <div className="input-group input-group-lg">
                        <div className="input-group-prepend">
                            <span className="input-group-text" id="inputGroup-search-adress">Address</span>
                        </div>
                        <input type="text" value={this.state.query} onChange={this.onInputChange} className="form-control" aria-label="Large" aria-describedby="inputGroup-sizing-sm" />
                    </div>


                        {this.state.data.length > 0 && (
                        <ul className="list-group list-group-flush">
                            {this.state.data.map(address =>
                                <li class="list-group-item" address-id={address.id}>{address.streetNumber} {address.streetName} {address.zipCode} {address.city}</li>
                                )}
                            </ul>
                        )}

                </div>
            </div>
        );
    }
};