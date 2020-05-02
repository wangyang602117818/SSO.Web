import React from 'react';
import { Grid } from 'antd-mobile';
import { withRouter } from "react-router-dom";
import '../css/manage.css';
class Manage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            datas:[
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#iconusers"></use></svg>,
                    text:'用户管理',
                    url:'/user'
                },
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#iconroles"></use></svg>,
                    text:'角色管理'
                },
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#iconcompany"></use></svg>,
                    text:'公司管理'
                },
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#icondepartment"></use></svg>,
                    text:'部门管理'
                },
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#iconnavigator"></use></svg>,
                    text:'导航管理'
                },
                {
                    icon:<svg className="icon" aria-hidden="true">
                    <use xlinkHref="#iconlog"></use></svg>,
                    text:'日志列表'
                }
            ]
        };
    }
    itemClick(item){
        this.props.history.push(item.url);
    }
    render() {
        return (
            <div className="manage">
                <div className="man_top">
                    <div className="man_title">管理中心</div>
                    <div className="man_news">xx新增了一条用户信息</div>
                </div>
                <div className="man_record">
                    <div className="man_record_item">
                        <div className="man_record_num">12243</div>
                        <div className="man_record_txt">总操作数</div>
                    </div>
                    <div className="man_record_item">
                    <div className="man_record_num">123</div>
                        <div className="man_record_txt">上月操作数</div>
                    </div>
                    <div className="man_record_item">
                    <div className="man_record_num">13</div>
                        <div className="man_record_txt">昨日操作数</div>
                    </div>
                </div>
                <div className="man_content">
                <Grid data={this.state.datas} onClick={this.itemClick.bind(this)}/>
                </div>
            </div>
        )
    }
}
export default withRouter(Manage);
