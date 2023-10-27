'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import { Auction, PagedResult } from '@/types';
import Filter from './Filter';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import queryString from 'query-string';
import EmptyFilter from '../components/EmptyFilter';

export default function Listings() {

    const [data, setData] = useState<PagedResult<Auction>>();
    const params = useParamsStore(state => ({
        pageSize: state.pageSize,
        pageNumber: state.pageNumber,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy
    }), shallow)
    const setParams = useParamsStore(state => state.setParams);
    const url = queryString.stringifyUrl({ url: '', query: params });

    const setPageNumber = (pageNumber: number) => setParams({ pageNumber });

    useEffect(() => {
        getData(url).then(data => {
            setData(data);
        })
    }, [url]);

    if (!data) return <h3>Loading...</h3>

    return (
        <>
            <Filter />
            {data.totalCount === 0 ? <EmptyFilter showReset /> : (
                <>
                    <div className='grid grid-cols-4 gap-6'>
                        {data?.results && data?.results.map((auction) => (
                            <AuctionCard key={auction.id} auction={auction} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination
                            pageChanged={setPageNumber}
                            currentPage={params.pageNumber}
                            totalCount={data?.pageCount ?? 4}
                        />
                    </div>
                </>
            )}
        </>
    )
}
