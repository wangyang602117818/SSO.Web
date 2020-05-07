import React from 'react';
import { NavBar, Icon, Popover, List, PullToRefresh, Toast } from 'antd-mobile';
import { withRouter } from 'react-router-dom';
import { axios, urls } from '../config/http';
import '../css/usermanage.css'
class UserManage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            datas: [],
            filter: ""
        };
        this.pageIndex = 1;
        this.pageSize = 15;
        this.isEnd = false;
        this.down = true;
        this.refreshing = false;
    }
    componentDidMount() {
        this.getData();
    }
    getData() {
        if (this.isEnd) {
            Toast.info('已到达末尾', 1);
            return;
        }
        axios.get(urls.user.getbasic + "?pageIndex=" + this.pageIndex + "&pageSize=" + this.pageSize + "&filter=" + this.state.filter).then(response => {
            this.refreshing = false;
            if (response.code === 0) {
                if (response.result.length === 0) {
                    this.isEnd = true;
                    if (this.pageIndex > 1) Toast.info('已到达末尾', 1);
                } else {
                    var res = this.state.datas.concat(response.result);
                    this.setState({ datas: res });
                }
            }
        });
    }
    getUserList(datas) {
        var array = [];
        datas.forEach((item) => {
            array.push(
                <List.Item
                    arrow="horizontal"
                    thumb="https://zos.alipayobjects.com/rmsportal/dNuvNrtqUztHCwM.png"
                    multipleLine
                    onClick={() => { }}
                    key={item._id}
                >
                    {item.UserName}
                    <List.Item.Brief>{item.CompanyName} | {item.DepartmentName}</List.Item.Brief>
                </List.Item>
            )
        });
        return array;
    }
    loadMore() {
        this.pageIndex = this.pageIndex + 1;
        this.refreshing = true;
        this.getData();
    }
    handleEnterKey(e) {
        this.isEnd = false;
        var code = e.nativeEvent.keyCode;
        if (code === 13) {
            this.setState({ datas: [] });
            this.pageIndex = 1;
            this.getData();
        }
    }
    render() {
        var dataExists = this.state.datas.length === 0 ? false : true;
        return (
            <div className="user_manage">
                <NavBar
                    mode="dack"
                    icon={<Icon type="left" />}
                    onLeftClick={() => { this.props.history.goBack(-1); }}
                    rightContent={
                        <Popover mask
                            visible={this.state.visible}
                            overlay={[
                                (<Popover.Item
                                    key="1"
                                    value="add"
                                    icon=
                                    {
                                        <svg className="icon" aria-hidden="true">
                                            <use xlinkHref="#iconadd"></use>
                                        </svg>
                                    }
                                    data-seed="logId">添加
                                </Popover.Item>)
                            ]}
                            align={{
                                overflow: { adjustY: 0, adjustX: 0 },
                                offset: [-10, 0],
                            }}
                            onVisibleChange={this.handleVisibleChange}
                            onSelect={this.onSelect}
                        >
                            <div style={{
                                height: '100%',
                                padding: '0 15px',
                                marginRight: '-15px',
                                display: 'flex',
                                alignItems: 'center',
                            }}
                            >
                                <Icon type="ellipsis" />
                            </div>
                        </Popover>
                    }
                >用户管理
            </NavBar>
                <div className="search_con">
                    <input type="text"
                        name="search"
                        placeholder="Search"
                        onChange={(e) => { this.setState({ filter: e.target.value }) }}
                        onKeyPress={this.handleEnterKey.bind(this)}
                        defaultValue={this.state.filter} />
                </div>
                <div className="list_con">
                    <div className="no_data" style={{ display: dataExists ? 'none' : 'block' }}>没有数据</div>
                    <List>
                        <PullToRefresh
                            damping={60}
                            direction='up'
                            refreshing={this.refreshing}
                            onRefresh={this.loadMore.bind(this)}
                        >
                            {this.getUserList(this.state.datas)}
                        </PullToRefresh>
                    </List>
                </div>
            </div>)
    }
}
export default withRouter(UserManage);