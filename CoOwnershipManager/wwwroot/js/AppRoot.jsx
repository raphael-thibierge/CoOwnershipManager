
const Loader = () => (
    <div className="text-center">
        <div className="spinner-border" role="status">
            <span className="sr-only">Loading...</span>
        </div>
    </div>
)

class NewPostBox extends React.Component {
    
    constructor(props) {
        super(props);
        this.state = this.getInitialState();
    }
    
    getInitialState = () => {
        return {title: '', content: '', enable: false};
    }
    
    triggerEnableButton = () => {this.setState({enable: !this.state.enable})}
    onTitleChange = (e) => {this.setState({title: e.target.value})}
    onContentChange = (e) => {this.setState({content: e.target.value})}
    
    onCreate = post => {
        if (typeof this.props.onPostCreate === 'function'){
            this.props.onPostCreate(post);
        }    
    }
    
    onPostClick = () => {
        // TODO : React Routing
        const uri = encodeURI('/api/Post/');
        const data = {
            Title: this.state.title,
            Content: this.state.content,
            BuildingId: this.props.building
        }
        console.log(data);
        // TODO : display loading etc... 
        // TODO : handle errors
        const xhr = new XMLHttpRequest();
        xhr.open('post', uri, true);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhr.onload = () => {
            const post = JSON.parse(xhr.responseText);
            this.setState(this.getInitialState(), this.onCreate.bind(null, post))
        };
        xhr.send(JSON.stringify(data));
        
    }
    
    render = () => {
        return (
            <div className="accordion" id="accordionExample">
                <div className="card">
                    <div className="card-header" id="headingOne">
                        
                            {!this.state.enable ? (
                                <button className="btn btn-outline-dark text-left" type="button" data-toggle="collapse"
                                        data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
                                        onClick={this.triggerEnableButton}
                                >New Post</button>
                            ):(
                                <input type="text"
                                    value={this.state.title}
                                    onChange={this.onTitleChange}
                                    placeholder={'Post title'}
                                    className={"form-control form-form-control-lg"}
                                    aria-label="Large" aria-describedby="inputGroup-new-post-title"
                                />
                            )}
                    </div>

                    <div id="collapseOne" className="collapse" aria-labelledby="headingOne"
                         data-parent="#accordionExample">
                        <div className="card-body">
                            <textarea value={this.state.content} onChange={this.onContentChange} rows="5" className="form-control" />
                        </div>
                        <div className="card-footer text-right">
                            
                            <button className="btn btn-outline-dark mr-2" onClick={this.triggerEnableButton} data-toggle="collapse"
                                    data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Cancel</button>
                            <button className="btn btn-success" onClick={this.onPostClick} data-toggle="collapse"
                                    data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Post !</button>
                        </div>
                    </div>
                </div>
                
            </div>
        )
    }
}

const  PostRender = (props) => (
    <div className="card mt-4">
        <div className="card-header">
            <h3>{props.post.title}</h3>
        </div>
        <div className="card-body">
            {props.post.content}
        </div>
        <div className="card-footer text-right">
            <div className="row">
            <div className="col-md-6 text-left">
                {props.post.postedAt.replace('T', ' ')}
            </div>
            <div className="col-md-6 text-right">
                {props.post.authorId}
            </div>
            </div>
        </div>
    </div>
)

const AppHeaderRender = (props) => (
    <div className="row">
        <div className="col-md-6">
            <h1>Building : {props.building.name}</h1>
            <h3>Apart. {props.apartment.number} / {props.apartments.length}: {props.apartment.description}</h3>
        </div>
        <div className="col-md-6 text-right mt-2">
            <p>{props.address.streetNumber} {props.address.streetName}</p>
            <p>{props.address.zipcode} {props.address.city} {props.address.country} </p>
            <p>{props.address.country} </p>
        </div>
    </div>
)



class AppRoot extends React.Component {
    
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            building: null,
            apartment: null,
            posts: null,
        }
    }

    componentDidMount() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', '/api/User', true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            this.setState({ 
                user: data,
                apartment: data.apartment,
                building: data.apartment.building,
                address: data.apartment.building.address,
                posts: data.apartment.building.posts['$values'].reverse(),
                apartments: data.apartment.building.apartments['$values'],
            });
        };
        xhr.send();
    }
    
    onPostCreate = (post) => {
        let posts = this.state.posts;
        console.log(typeof posts)
        posts.unshift(post);
        this.setState({posts: posts})
    }
    
    render = () => {
        if (this.state.user === null){
            return <Loader />;
        }        
        
        return (
            <div className="row">
                <div className="col-12">
                    
                    <div className="row">
                        <div className="col-12">
                            <AppHeaderRender
                                apartment={this.state.apartment}
                                apartments={this.state.apartments}
                                building={this.state.building} 
                                address={this.state.address} 
                            />
                            
                        </div>
                    </div>
                    
                    <hr/>
                    
                    <div className="row">
                        <div className="col-12">
                            <NewPostBox
                                building={this.state.building.id}
                                onPostCreate={this.onPostCreate}
                            />
                        </div>
                    </div>
                    
                    <hr/>
                    <div className="row">
                        <div className="col-12">
                            {this.state.posts.map(p => (
                                <div className="row" key={p.id}>
                                    <div className="col-12">
                                        <PostRender post={p} />
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                    
                </div>
            </div>
        )
    }
}