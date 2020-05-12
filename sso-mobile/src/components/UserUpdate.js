import React from 'react';
import { NavBar, Icon, InputItem, List, Picker } from 'antd-mobile';
import { withRouter } from 'react-router-dom';
import { axios, urls } from '../config/http';
import '../css/updateuser.css'
class UserUpdate extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            User: {},
            companyData: []
        };
        this.sex = [{
            label: '男',
            value: 'M'
        }, {
            label: '女',
            value: 'F'
        }]
    }
    componentDidMount() {
        this.getUserData();
        this.getCompanyData();
    }
    getUserData() {
        var id = this.props.match.params.id;
        axios.get(urls.user.getbyuserid + "?userid=" + id)
            .then(response => {
                if (response.code === 0) {
                    this.setState({ User: response.result });
                }
            })
    }
    getCompanyData() {
        axios.get(urls.company.getall)
            .then(response => {
                if (response.code === 0) {
                    this.setState({ companyData: this.assembleCompany(response.result) });
                }
            });
    }
    assembleCompany(comps) {
        var result = [];
        comps.forEach(function (item) {
            result.push({
                label: item.Name,
                value: item.Code
            })
        })
        return result;
    }
    onChangeSex(sex) {
        this.state.User.Sex = sex[0];
        this.setState({ User: this.state.User });
    }
    onChangeCompany(company) {
        this.state.User.CompanyCode = company[0];
        this.setState({ User: this.state.User });
    }
    render() {
        return (<div className="user_update">
            <NavBar
                mode="dark"
                icon={<Icon type="left" />}
                onLeftClick={() => { this.props.history.goBack(-1); }}
            >用户修改</NavBar>
            <List renderHeader={() => '修改之后点击保存'}>
                <InputItem
                    clear
                    placeholder="编号"
                >编号</InputItem>
                <InputItem
                    clear
                    placeholder="用户名称"
                >用户名</InputItem>
                <Picker data={this.sex}
                    cols={1}
                    title="选择性别"
                    onPickerChange={this.onChangeSex.bind(this)}
                    value={[this.state.User.Sex || 'M']}>
                    <List.Item arrow="horizontal">性别</List.Item>
                </Picker>
                <InputItem
                    type="phone"
                    placeholder="手机号码"
                >手机号码</InputItem>
                <InputItem
                    clear
                    placeholder="邮箱"
                >邮箱</InputItem>
                <InputItem
                    clear
                    placeholder="身份证号"
                >身份证号</InputItem>
                <Picker data={this.state.companyData}
                    cols={1}
                    title="公司"
                    onPickerChange={this.onChangeCompany.bind(this)}
                    value={[this.state.User.CompanyCode]}>
                    <List.Item arrow="horizontal">公司</List.Item>
                </Picker>
                <List.Item
                    multipleLine
                    extra={this.state.User.DepartmentName?.join(',')}
                    arrow="horizontal"
                    onClick={() => { }}>部门</List.Item>
            </List>
        </div>)
    }
}
export default withRouter(UserUpdate);