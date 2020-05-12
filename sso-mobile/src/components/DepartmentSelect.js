import React from 'react';
import { NavBar, List } from 'antd-mobile';
import { withRouter } from 'react-router-dom';
import { axios, urls } from '../config/http';
import '../css/departmentselect.css'
class DepartmentSelect extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            departmentData: []
        };
    }
    getDepartmentData() {
        var companyCode = this.props.match.params.companycode;
        console.log(companyCode);
        axios.get(
            urls.department.getdepartments + "?companyCode=" + companyCode
        ).then(response => {
            if (response.code === 0) {
                if (response.result.length > 0) {
                    this.setState({ departmentData: response.result });
                } else {
                    this.setState({ departmentData: [] });
                }
            }
        });
    }
    render() {
        return (<div>select</div>)
    }
}
export default withRouter(DepartmentSelect);