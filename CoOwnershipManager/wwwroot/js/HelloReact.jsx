// Hello world react component to test config
const HelloReact = props => {
    return (
    <div className="row">
        <div className="col-md-6 offset-md-3">
            <div className="alert alert-success">React : {props.message}</div>
        </div>
    </div>
    );
};