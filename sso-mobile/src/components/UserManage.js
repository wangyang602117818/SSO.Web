import React from 'react';
import { NavBar, Icon } from 'antd-mobile';
import { withRouter } from 'react-router-dom';
class UserManage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }
    render() {
        return (<div>
            <NavBar
                mode="light"
                icon={<Icon type="left" />}
                onLeftClick={() => { this.props.history.goBack(-1); }}
            >用户管理</NavBar>
        </div>)
    }
}

export default withRouter(UserManage);